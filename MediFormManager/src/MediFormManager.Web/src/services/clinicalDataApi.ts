export type MappedClinicalValues = Record<string, string>;

export async function getMappedClinicalValues(
    formVersionId: string,
    patientId: string
): Promise<MappedClinicalValues> {
    const token = sessionStorage.getItem("accessToken");

    const response = await fetch(
        `https://localhost:7276/api/clinical-data/form-versions/${encodeURIComponent(formVersionId)}/patients/${encodeURIComponent(patientId)}/mapped-values`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    if (!response.ok) {
        throw new Error(
            `Failed to load clinical data: HTTP ${response.status}`
        );
    }

    return (await response.json()) as MappedClinicalValues;
}