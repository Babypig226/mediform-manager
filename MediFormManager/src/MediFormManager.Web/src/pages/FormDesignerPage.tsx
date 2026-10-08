import {useState} from "react";
import type {FormComponent, FormSchema, ComponentOption } from "../types/formSchema";

interface FormDesignerPageProps{
    schema: FormSchema;
}

export default function FormDesignerPage({schema} : FormDesignerPageProps) {
    const [selectedComponentId, setSelectedComponentId] = useState<string | null>(null);
    const [draftSchema, setDraftSchema] = useState<FormSchema>(() => structuredClone(schema));
    const [newComponentType, setNewComponentType] = useState<string>("TextInput");
    const selectedComponent = draftSchema.components.find(
        component => component.id === selectedComponentId
    );

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

    return (
        <section className = "designer">
            <h2>Form Designer</h2>            

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

                    {draftSchema.components.map(component => (
                        <button
                            key={component.id}
                            type = "button"
                            className={
                                selectedComponentId === component.id
                                    ? "designer-component selected"
                                    : "designer-component"
                            }
                            onClick={() => setSelectedComponentId(component.id)}
                        >
                            <strong>
                                {component.label ?? component.prompt ?? "Untitled"}
                            </strong>
                            <p>{component.componentType}</p>
                        </button>
                    ))}
                </main>

                <aside className="designer-properties">
                    <h3>Properties</h3>

                    {selectedComponent ? (
                       <>
                            <p>
                                <strong>Type:</strong> {selectedComponent.componentType}
                            </p>

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
                        </>
                    ) : (
                        <p>Select a component to edit its properties.</p>
                    )}
                </aside>
            </div>
        </section>
    );
}