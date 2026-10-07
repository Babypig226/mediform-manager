import {useState} from "react";
import type { FormComponent, FormSchema, RuleCondition, ComponentRule, RuleAction } from "../types/formSchema";

interface FormRendererProps {
    schema: FormSchema;
}

interface ComponentRuntimeState{
    isDisabled: boolean;
    isRequired: boolean;
    isVisible: boolean;
}

export function FormRenderer({ schema }: FormRendererProps) {
    const [values, setValues] = useState<Record<string, string>>({});
    const [validationErrors, setValidationErrors] = useState<string[]>([]);
    const [hasValidated, setHasValidated] = useState(false);
    function renderComponent(component: FormComponent, runtimeState: ComponentRuntimeState) {
        switch (component.componentType) {
            case "TextInput":
                return (
                    <input
                        type="text"
                        placeholder={component.placeholder ?? ""}                        
                        value={values[component.id] ?? component.defaultValue ?? ""}
                        onChange={(e) => handleValueChange(component.id, e.target.value)}
                        disabled={runtimeState.isDisabled}
                    />
                );

            case "RadioGroup":
                return (
                    <div className="radio-group">
                        {component.options.map(option => (
                            <label key={option.id} className="radio-option">
                                <input
                                    type="radio"
                                    name={component.id}
                                    value={option.id}
                                    checked={values[component.id] === option.id}
                                    onChange={() => handleValueChange(component.id, option.id)}
                                    disabled={runtimeState.isDisabled}
                                />
                                {option.displayText}
                            </label>
                        ))}
                    </div>
                );

            case "ComboBox":
                return (
                    <select disabled={runtimeState.isDisabled}>
                        <option value="">Select...</option>

                        {component.options.map(option => (
                            <option key={option.id} value={option.id}>
                                {option.displayText}
                            </option>
                        ))}
                    </select>
                );

            case "DateTimePicker":
                return (
                    <input
                        type="datetime-local"
                        disabled={runtimeState.isDisabled}
                    />
                );

            case "CheckBox":
                return (
                    <input
                        type="checkbox"
                        disabled={runtimeState.isDisabled}
                    />
                );

            case "Label":
                return <p>{component.prompt}</p>;

            default:
                return (
                    <p>
                        Unsupported component: {component.componentType}
                    </p>
                );
        }
    }

    function handleValueChange(componentId: string, value: string){
        setValues(previousValues =>{
            const nextValues = {
                ...previousValues,
                [componentId]: value
            };
             return applyClearActions(nextValues);
        });

        setValidationErrors([]);
        setHasValidated(false);
    }

   function handleValidate() {
        const errors = validateForm();

        setValidationErrors(errors);
        setHasValidated(true);
    }

    function evaluateCondition(condition: RuleCondition, currentValues: Record<string, string>): boolean {
        const currentValue = currentValues[condition.sourceComponentId];
        const expectedValue = condition.expectedOptionId ?? condition.expectedValue;
        switch(condition.operator){
            case "Equals" :
                
                return currentValue === expectedValue;
            case "NotEquals" :
                return currentValue !== expectedValue;
            case "IsEmpty" :
                return currentValue === "" || currentValue === undefined || currentValue === null;
            case "IsNotEmpty" :
                return currentValue !== "" && currentValue !== undefined && currentValue !== null;
            default:
                return false;
        }
    }

    function evaluateRule(rule: ComponentRule, currentValues: Record<string, string>):boolean{
        if (rule.conditions.length === 0) {
            return false;
        }
        switch(rule.logic){
            case "And":
                return rule.conditions.every(condition => evaluateCondition(condition, currentValues));
            case "Or":
                return rule.conditions.some(condition => evaluateCondition(condition, currentValues));
            default :
                return false;
        }
    }

    function createBaseRuntimeStates():Record<string, ComponentRuntimeState>{
        return schema.components.reduce(
            (states, component) =>{
                states[component.id] = {
                    isDisabled: component.isDisabled,
                    isRequired: component.isRequired,
                    isVisible: component.isVisible
                };
                return states;
            },
            {} as Record<string, ComponentRuntimeState>
        );
    }

    function applyAction(action: RuleAction, states: Record<string, ComponentRuntimeState>){
        if (
                action.targetType === "Component"
                && action.targetComponentId
            ) {
                const targetState = states[action.targetComponentId];

                if (!targetState) {
                    return;
                }
                switch (action.actionType) {

                    case "Enable":
                        targetState.isDisabled = false;
                        break;

                    case "Disable":
                        targetState.isDisabled = true;
                        break;

                    case "Required":
                        targetState .isRequired = true  ;
                        break;

                    case "Optional":
                        targetState.isRequired = false;
                        break;
                    default:
                        break;
                }
            }
        
    }

    function applyClearActions(
        currentValues: Record<string, string>
    ): Record<string, string> {

        const nextValues = { ...currentValues };

        schema.rules.forEach(rule => {
            if (!evaluateRule(rule, currentValues)) {
                return;
            }

            rule.actions.forEach(action => {
                if (
                    action.actionType === "Clear" &&
                    action.targetType === "Component" &&
                    action.targetComponentId
                ) {
                    nextValues[action.targetComponentId] = "";
                }
            });
        });

        return nextValues;
    }

    function validateForm(): string[]{
        const errors: string[] = [];
        schema.components.forEach(component => {
            const runtimeState = runtimeStates[component.id];
            if(runtimeState.isRequired && (!values[component.id] || values[component.id].trim() === "")){
                errors.push(`Field "${component.label ?? component.prompt}" is required.`);
            }
        });
        return errors;
    }

    function calculateRuntimeStates():Record<string, ComponentRuntimeState>{
        const states = createBaseRuntimeStates();

        schema.rules.forEach(rule => {
            const ruleResult = evaluateRule(rule, values);
            if(ruleResult){
                rule.actions.forEach(action => {
                    applyAction(action, states);
                });
            }
        });

        return states;
    }

    
    const runtimeStates = calculateRuntimeStates();
    return (
        <div className="form-renderer">
            <div className="form-header">
                <div>
                    <span className="form-eyebrow">MEDICAL FORM</span>
                    <h2>Consent Form</h2>
                </div>

                <span className="form-version">
                    v{schema.version}
                </span>
            </div>

            <div className="form-body">
                {schema.components
                    .filter(component => runtimeStates[component.id]?.isVisible)
                    .map(component => (
                        <div
                            key={component.id}
                            className="form-field"
                        >
                            {(component.label || component.prompt) && (
                                <label className="field-label">
                                    {component.label ?? component.prompt}

                                    {runtimeStates[component.id]?.isRequired && (
                                        <span className="required">
                                            *
                                        </span>
                                    )}
                                </label>
                            )}

                            {component.prompt &&
                                component.label &&
                                component.prompt !== component.label && (
                                    <p className="field-prompt">
                                        {component.prompt}
                                    </p>
                                )}

                            {renderComponent(component, runtimeStates[component.id])}
                        </div>
                    ))}

                <button
                    type="button"
                    onClick={handleValidate}
                >
                    Validate
                </button>

                {hasValidated && (
                    validationErrors.length > 0 ? (
                        <div className="validation-errors">
                            {validationErrors.map((error, index) => (
                                <p key={index}>{error}</p>
                            ))}
                        </div>
                    ) : (
                        <div className="validation-success">
                            All required fields are complete.
                        </div>
                    )
                )}
            </div>
        </div>
    );
}