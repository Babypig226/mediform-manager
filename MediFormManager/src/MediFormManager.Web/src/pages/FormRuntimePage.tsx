import { useEffect, useState } from "react";
import { FormRenderer } from "../components/FormRenderer";
import { getMappedClinicalValues } from "../services/clinicalDataApi";
import type { FormSchema } from "../types/formSchema";

interface FormRuntimePageProps {
    schema: FormSchema;
}

const DEMO_PATIENT_ID = "P001";

export default function FormRuntimePage({
    schema
}: FormRuntimePageProps) {
    const [initialValues, setInitialValues] =
        useState<Record<string, string> | null>(null);

    const [loadError, setLoadError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;

        async function loadClinicalData() {
            setInitialValues(null);
            setLoadError(null);

            try {
                const values = await getMappedClinicalValues(
                    schema.formVersionId,
                    DEMO_PATIENT_ID
                );

                if (!cancelled) {
                    setInitialValues(values);
                }
            } catch (error) {
                if (!cancelled) {
                    setLoadError(
                        error instanceof Error
                            ? error.message
                            : "Failed to load clinical data."
                    );
                }
            }
        }

        void loadClinicalData();

        return () => {
            cancelled = true;
        };
    }, [schema.formVersionId]);

    if (loadError) {
        return (
            <div role="alert">
                <p>{loadError}</p>
                <button
                    type="button"
                    onClick={() => window.location.reload()}
                >
                    Reload
                </button>
            </div>
        );
    }

    if (initialValues === null) {
        return <p>Loading patient data...</p>;
    }

    return (
        <FormRenderer
            key={`${schema.formVersionId}-${DEMO_PATIENT_ID}`}
            schema={schema}
            initialValues={initialValues}
        />
    );
}