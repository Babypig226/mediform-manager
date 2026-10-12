export interface DataDictionaryItem {
    id: string;
    dataKey: string;
    displayName: string;
    dataType: string;
}

export async function getDataDictionary(): Promise<DataDictionaryItem[]> {
    const token = sessionStorage.getItem("accessToken");

    const response = await fetch(
        "https://localhost:7276/api/data-dictionary",
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    if (!response.ok) {
        throw new Error(
            `Failed to load data dictionary: HTTP ${response.status}`
        );
    }

    return (await response.json()) as DataDictionaryItem[];
}