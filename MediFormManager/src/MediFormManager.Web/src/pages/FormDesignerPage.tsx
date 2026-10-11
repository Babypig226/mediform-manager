import {useState} from "react";
import type {FormComponent, FormSchema, ComponentOption } from "../types/formSchema";
import { FormField } from "../components/FormField";
import { FormRenderer } from "../components/FormRenderer";
import { getFormSchema, saveFormSchema } from "../services/formSchemaApi";

interface FormDesignerPageProps{
    schema: FormSchema;
}

export default function FormDesignerPage({schema} : FormDesignerPageProps) {
    const [selectedComponentId, setSelectedComponentId] = useState<string | null>(null);
    const [draftSchema, setDraftSchema] = useState<FormSchema>(() => structuredClone(schema));
    const [newComponentType, setNewComponentType] = useState<string>("TextInput");
    const [isPreview, setIsPreview] = useState(false);
    const selectedComponent = draftSchema.components.find(
        component => component.id === selectedComponentId
    );
    const [isSaving, setIsSaving] = useState(false);
    const [saveMessage, setSaveMessage] = useState<string | null>(null);

    function updateComponent(componentId: string, changes: Partial<FormComponent>){
        setDraftSchema(previous => ({...previous, 
            components: previous.components.map(component =>
                component.id === componentId ? {...component, ...changes}
                : component
            )
        }));
    }

    function addComponent(componentType: string){
        const newComponent: FormComponent ={
            id: crypto.randomUUID(),
            componentType,
            label: `New ${componentType}`,
            prompt: null,
            placeholder: "",
            defaultValue: null,
            isRequired: false,
            isDisabled: false,
            isVisible: true,
            groupKey: null,
            order: Math.max(-1, ...draftSchema.components.map(c => c.order)) + 1,
            options: []
        };
        setDraftSchema(previous => ({
            ...previous,
            components: [...previous.components, newComponent]
        }));

        setSelectedComponentId(newComponent.id);
    }

    function addOption(componentId: string) {
        const component = draftSchema.components.find(component => component.id === componentId);
        if (
                !component ||
                !["RadioGroup", "ComboBox"].includes(component.componentType)
            ) {
                return;
            }

        const newOption: ComponentOption ={
            id: crypto.randomUUID(),
            displayText: "New Option",
            order: Math.max(-1, ...component.options.map(option => option.order)) + 1
        }

        updateComponent(componentId, {options: [...component.options, newOption]});
    }

    function updateOption(componentId: string, optionId: string, displayText: string){
        const component = draftSchema.components.find(component => component.id === componentId);

        if(!component) return;

        updateComponent(componentId, {
            options: component.options.map(option =>
                option.id === optionId?{...option, displayText} : option
            )
        });

    }

    function removeOption(componentId: string, optionId: string){
        const component = draftSchema.components.find(component => component.id === componentId);
        if(!component) return;
        updateComponent(componentId, {
            options: component.options.filter(option => option.id !== optionId)
        });
    }

    async function handleSave() {
            if (isSaving) return;

            setIsSaving(true);
            setSaveMessage(null);

            try {
                await saveFormSchema(draftSchema);

                const latestSchema = await getFormSchema(draftSchema.formVersionId);

                setDraftSchema(latestSchema);
                setSaveMessage("Schema saved successfully.");
            } catch (err) {
                setSaveMessage(
                    err instanceof Error ? err.message : "Save failed."
                );
            } finally {
                setIsSaving(false);
            }
        }

        function removeComponent(componentId: string){
            const referencedByRule = draftSchema.rules.some(rule => rule.conditions.some(condition => condition.sourceComponentId === componentId) 
                                                                    || 
                                                                    rule.actions.some(action => action.targetComponentId === componentId));
            if(referencedByRule){
                setSaveMessage("Cannot delete a component referenced by a rule.");
                return;
            }

            setDraftSchema(previous => ({
                ...previous,
                components: previous.components
                            .filter(component => component.id !== componentId)
                            .map((component, index)=>({
                                ...component,
                                order:index
                            }))
            }));

            setSelectedComponentId(null);
            setSaveMessage(null);
            

        }

        function moveComponent(componentId:string, direction:"up"|"down"){
            setDraftSchema(previous => {
                const components = [...previous.components].sort((a,b)=> a.order - b.order);
                const currentIndex = components.findIndex(component => component.id === componentId);
                if(currentIndex === -1) return previous;
                const targetIndex = direction ==="up"? currentIndex -1 : currentIndex +1;
                if(targetIndex < 0 || targetIndex >= components.length){
                    return previous;
                }
                [components[currentIndex], components[targetIndex]] = [components[targetIndex], components[currentIndex]];
                return{
                    ...previous, components: components.map((component, index)=> ({...component, order:index}))                    
                };
            });

          
        }

    return (
        <section className = "designer">
            <div className="designer-heading">
                <h2>Form Designer</h2>
                <span>Draft · Version {draftSchema.version}</span>
            </div>

            <div className="designer-actions">
                <button
                    type="button"
                    className="designer-preview-button"
                    onClick={() => setIsPreview(previous => !previous)}
                >
                    {isPreview ? "← Back to Designer" : "Preview Draft"}
                </button>

                {!isPreview && (
                    <button
                        type="button"
                        className="designer-save-button"
                        onClick={handleSave}
                        disabled={isSaving}
                    >
                        {isSaving ? "Saving..." : "Save Draft"}
                    </button>
                )}
            </div>
            

            {saveMessage && <p role="status">{saveMessage}</p>}            

            {isPreview ? (
                <div className="designer-preview">
                    <FormRenderer schema={draftSchema} />
                </div>
            ) : (
                <div className="designer-layout">
                    <aside className="designer-components">
                        <h3>Components</h3>

                        {draftSchema.components.map(component => (
                            <button
                                key={component.id}
                                type="button"
                                onClick={() => setSelectedComponentId(component.id)}
                            >
                                {component.label ?? component.prompt ?? component.componentType}
                            </button>

                        ))}
                        <div className="add-component-controls">
                            <select
                                value={newComponentType}
                                onChange={e => setNewComponentType(e.target.value)}
                            >
                                <option value="TextInput">Text Input</option>
                                <option value="RadioGroup">Radio Group</option>
                                <option value="ComboBox">Combo Box</option>
                                <option value="DateTimePicker">Date/Time Picker</option>
                                <option value="CheckBox">Check Box</option>
                                <option value="Label">Label</option>
                            </select>



                            <button
                                type="button"
                                className="add-component-button"
                                onClick={() => addComponent(newComponentType)}
                            >
                                + Add Component
                            </button>
                        </div>
                    </aside>

                    <main className="designer-canvas">
                        <h3>Canvas</h3>

                        {draftSchema.components
                                    .slice()
                                    .sort((a, b) => a.order - b.order)
                                    .map(component => (
                                        <div
                                            key={component.id}
                                            className={
                                                selectedComponentId === component.id
                                                    ? "designer-component selected"
                                                    : "designer-component"
                                            }
                                        >
                                            <div
                                                className="designer-field-selector"
                                                role="button"
                                                tabIndex={0}
                                                onClick={() => setSelectedComponentId(component.id)}
                                                onKeyDown={e => {
                                                    if (e.key === "Enter" || e.key === " ") {
                                                        e.preventDefault();
                                                        setSelectedComponentId(component.id);
                                                    }
                                                }}
                                            >
                                                <FormField
                                                    component={component}
                                                    isDisabled={component.isDisabled}
                                                    isRequired={component.isRequired}
                                                    mode="designer"
                                                />
                                            </div>
                                        </div>
                                    ))}
                    </main>

                    <aside className="designer-properties">
                        <h3>Properties</h3>

                        {selectedComponent ? (
                        <>
                                <p>
                                    <strong>Type:</strong> {selectedComponent.componentType}
                                </p>
                                <div className="component-order-controls">
                                <button
                                    type="button"
                                    onClick={() => moveComponent(selectedComponent.id, "up")}
                                    disabled={
                                        draftSchema.components.findIndex(
                                            c => c.id === selectedComponent.id
                                        ) === 0
                                    }
                                >
                                    ↑ Up
                                </button>

                                <button
                                    type="button"
                                    onClick={() => moveComponent(selectedComponent.id, "down")}
                                    disabled={
                                        draftSchema.components.findIndex(
                                            c => c.id === selectedComponent.id
                                        ) === draftSchema.components.length - 1
                                    }
                                >
                                    ↓ Down
                                </button>
                            </div>

                                <div className="property-field">
                                <label htmlFor="component-label">Label</label>
                                <input
                                    id="component-label"
                                    type="text"
                                    value={selectedComponent.label ?? ""}
                                    onChange={e =>
                                        updateComponent(selectedComponent.id, {
                                            label: e.target.value
                                        })
                                    }
                                />
                                </div>

                                <div className="property-field">
                                    <label htmlFor="component-prompt">Prompt</label>
                                    <input
                                        id="component-prompt"
                                        type="text"
                                        value={selectedComponent.prompt ?? ""}
                                        onChange={e =>
                                            updateComponent(selectedComponent.id, {
                                                prompt: e.target.value
                                            })
                                        }
                                    />
                                </div>

                                <div className="property-field">
                                    <label htmlFor="component-placeholder">Placeholder</label>
                                    <input
                                        id="component-placeholder"  
                                        type="text"
                                        value={selectedComponent.placeholder ?? ""}
                                        onChange={e =>
                                            updateComponent(selectedComponent.id, {
                                                placeholder: e.target.value
                                            })
                                        }
                                    />
                                </div>

                                <div className="property-field">
                                    <label className="property-checkbox">
                                        <input
                                            type="checkbox"
                                            checked={selectedComponent.isRequired}
                                            onChange={e =>
                                                updateComponent(selectedComponent.id, {
                                                    isRequired: e.target.checked
                                                })
                                            }
                                        />
                                        Required
                                    </label>
                                </div>
                                {["RadioGroup", "ComboBox"].includes(
                                        selectedComponent.componentType
                                    ) && (
                                        <div className="property-options">
                                            <h4>Options</h4>

                                            {selectedComponent.options.map(option => (
                                                <div className="property-option-row" key={option.id}>
                                                    <input
                                                        type="text"
                                                        value={option.displayText}
                                                        onChange={e =>
                                                            updateOption(
                                                                selectedComponent.id,
                                                                option.id,
                                                                e.target.value
                                                            )
                                                        }
                                                    />

                                                    <button
                                                        type="button"
                                                        onClick={() =>
                                                            removeOption(selectedComponent.id, option.id)
                                                        }
                                                    >
                                                        Delete
                                                    </button>
                                                </div>
                                            ))}

                                            

                                            <button
                                                type="button"
                                                onClick={() => addOption(selectedComponent.id)}
                                            >
                                                + Add Option
                                            </button>
                                            
                                        </div>
                                    )}
                            <button
                                                type="button"
                                                className="delete-component-button"
                                                onClick={() => removeComponent(selectedComponent.id)}
                                            >
                                                Delete Component
                                            </button>
                            </>

                        
                        ) : (
                            <p>Select a component to edit its properties.</p>
                        )}
                    </aside>
                </div>
            )}
        </section>
    );
}