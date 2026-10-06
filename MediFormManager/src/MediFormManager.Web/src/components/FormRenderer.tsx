import type { FormComponent, FormSchema } from "../types/formSchema";

interface FormRendererProps {
    schema: FormSchema;
}

export function FormRenderer({ schema }: FormRendererProps) {
    function renderComponent(component: FormComponent) {
        switch (component.componentType) {
            case "TextInput":
                return (
                    <input
                        type="text"
                        placeholder={component.placeholder ?? ""}
                        defaultValue={component.defaultValue ?? ""}
                        disabled={component.isDisabled}
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
                                    disabled={component.isDisabled}
                                />
                                {option.displayText}
                            </label>
                        ))}
                    </div>
                );

            case "ComboBox":
                return (
                    <select disabled={component.isDisabled}>
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
                        disabled={component.isDisabled}
                    />
                );

            case "CheckBox":
                return (
                    <input
                        type="checkbox"
                        disabled={component.isDisabled}
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
                    .filter(component => component.isVisible)
                    .map(component => (
                        <div
                            key={component.id}
                            className="form-field"
                        >
                            {(component.label || component.prompt) && (
                                <label className="field-label">
                                    {component.label ?? component.prompt}

                                    {component.isRequired && (
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

                            {renderComponent(component)}
                        </div>
                    ))}
            </div>
        </div>
    );
}