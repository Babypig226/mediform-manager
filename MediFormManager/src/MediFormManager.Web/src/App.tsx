import { useState } from "react";
import type { FormSchema } from "./types/formSchema";
import { getFormSchema } from "./services/formSchemaApi";
import { login } from "./services/authApi";
import "./App.css";
import { FormRenderer } from "./components/FormRenderer";

const FORM_VERSION_ID = "47571803-b7db-4c77-a2b6-61455dcf861c";

function App() {
    const [loginId, setLoginId] = useState("");
    const [password, setPassword] = useState("");
    const [schema, setSchema] = useState<FormSchema | null>(null);
    const [error, setError] = useState<string | null>(null);

    async function handleLogin() {
      try {
        setError(null);

        const result = await login({
          loginId: loginId,
          password: password
        });
        console.log("Login Response:", result);
        sessionStorage.setItem("accessToken", result.accessToken);

        const formSchema = await getFormSchema(FORM_VERSION_ID);
        setSchema(formSchema);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown error");
      }
    }

    

    return (
    <div className="app">
        <h1 className="app-title">MediForm Manager</h1>

        <div>
            <input
                type="text"
                placeholder="Login ID"
                value={loginId}
                onChange={(e) => setLoginId(e.target.value)}
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <button onClick={handleLogin}>
                Login
            </button>
        </div>

        {error && <p>{error}</p>}        
        {schema && (
            <FormRenderer schema={schema} />
        )}
    </div>
);
}

export default App;