import type { FormComponent } from "../types/formSchema";

interface FormFieldProps {
    component: FormComponent;
    value?: string;
    isDisabled: boolean;
    isRequired: boolean;
    mode?: "runtime" | "designer";
    onValueChange?: (componentId: string, value: string) => void;
}

export function FormField({
    component,
    value,
    isDisabled,
    isRequired,
    mode = "runtime",
    onValueChange
}: FormFieldProps) {
    const isDesigner = mode === "designer";
    const disabled = isDesigner || isDisabled;
    const currentValue = value ?? component.defaultValue ?? "";

    function changeValue(nextValue: string) {
        if (!isDesigner) {
            onValueChange?.(component.id, nextValue);
        }
    }

    function renderInput() {
        switch (component.componentType) {
            case "TextInput":
                return (
                    <input
                        type="text"
                        placeholder={component.placeholder ?? ""}
                        value={isDesigner ? "" : currentValue}
                        onChange={e => changeValue(e.target.value)}
                        disabled={disabled}
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
                                    checked={!isDesigner && currentValue === option.id}
                                    onChange={() => changeValue(option.id)}
                                    disabled={disabled}
                                />
                                {option.displayText}
                            </label>
                        ))}
                    </div>
                );

            case "ComboBox":
                if (isDesigner) {
                    return (
                        <div className="designer-select-preview">
                            <div className="designer-select-header">
                                <span>Select...</span>
                                <span aria-hidden="true">▾</span>
                            </div>

                            <div className="designer-select-options">
                                {component.options.length === 0 ? (
                                    <span className="designer-empty-option">
                                        No options configured
                                    </span>
                                ) : (
                                    component.options.map(option => (
                                        <div key={option.id}>
                                            {option.displayText || "(Empty option)"}
                                        </div>
                                    ))
                                )}
                            </div>
                        </div>
                    );
                }

                return (
                    <select
                        value={currentValue}
                        onChange={e => changeValue(e.target.value)}
                        disabled={disabled}
                    >
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
                        value={isDesigner ? "" : currentValue}
                        onChange={e => changeValue(e.target.value)}
                        disabled={disabled}
                    />
                );

            case "CheckBox":
                return (
                    <input
                        type="checkbox"
                        checked={!isDesigner && currentValue === "true"}
                        onChange={e => changeValue(String(e.target.checked))}
                        disabled={disabled}
                    />
                );

            case "Label":
                return <p>{component.prompt}</p>;

            default:
                return <p>Unsupported component: {component.componentType}</p>;
        }
    }

    return (
        <div className="form-field">
            {(component.label || component.prompt) && (
                <div className="field-label">
                    {component.label ?? component.prompt}
                    {isRequired && <span className="required">*</span>}
                </div>
            )}

            {component.prompt &&
                component.label &&
                component.prompt !== component.label && (
                    <p className="field-prompt">{component.prompt}</p>
                )}

            {renderInput()}
        </div>
    );
}