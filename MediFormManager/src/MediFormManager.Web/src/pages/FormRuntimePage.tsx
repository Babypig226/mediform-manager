import {FormRenderer} from "../components/FormRenderer";
import type { FormSchema } from "../types/formSchema";

interface FormRuntimePageProps {
    schema: FormSchema;
}

export default function FormRuntimePage({
    schema
}: FormRuntimePageProps) {
    return <FormRenderer schema={schema} />;
}