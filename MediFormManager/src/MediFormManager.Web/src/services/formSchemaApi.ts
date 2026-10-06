import type{ FormSchema } from '../types/formSchema';

export async function getFormSchema(id: string): Promise<FormSchema> {
    const token = sessionStorage.getItem("accessToken");

    if (!token) {
        throw new Error("Access token not found.");
    }
    const response = await fetch(`https://localhost:7276/api/FormVersions/${id}/schema`,
       { headers: {
            Authorization: `Bearer ${token}`
        }
    }
    );

    if(!response.ok) {
        throw new Error(`Failed to load form schema: ${response.status}`);
    }

    const schema: FormSchema = await response.json();

    return schema;
}