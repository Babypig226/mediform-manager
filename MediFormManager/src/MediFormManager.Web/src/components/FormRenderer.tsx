import {useState} from "react";
import type { FormSchema, RuleCondition, ComponentRule, RuleAction } from "../types/formSchema";
import { FormField } from "./FormField";

interface FormRendererProps {
    schema: FormSchema;
     initialValues?: Record<string, string>;
}

interface ComponentRuntimeState{
    isDisabled: boolean;
    isRequired: boolean;
    isVisible: boolean;
}

export function FormRenderer({schema, initialValues = {}}:FormRendererProps) {
    const [values, setValues] = useState<Record<string, string>>(() => ({...initialValues}));
    const [validationErrors, setValidationErrors] = useState<string[]>([]);
    const [hasValidated, setHasValidated] = useState(false);
    



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
            if(  runtimeState.isVisible &&
                !runtimeState.isDisabled &&
                runtimeState.isRequired &&
                !(values[component.id] ?? component.defaultValue ?? "").trim()
            ){
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
                            <FormField
                                key={component.id}
                                component={component}
                                value={values[component.id]}
                                isDisabled={runtimeStates[component.id].isDisabled}
                                isRequired={runtimeStates[component.id].isRequired}
                                onValueChange={handleValueChange}
                            />
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