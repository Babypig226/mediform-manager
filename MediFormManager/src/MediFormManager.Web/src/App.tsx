import { useState } from "react";
import type { FormSchema } from "./types/formSchema";
import { getFormSchema } from "./services/formSchemaApi";
import { login } from "./services/authApi";
import "./App.css";
import FormRuntimePage from "./pages/FormRuntimePage";
import FormDesignerPage from "./pages/FormDesignerPage";

const FORM_VERSION_ID = "08ea1080-1220-4ab3-9c13-6599e1a041bd";

function App() {
    const [loginId, setLoginId] = useState("");
    const [password, setPassword] = useState("");
    const [schema, setSchema] = useState<FormSchema | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [activePage, setActivePage] = useState<"runtime" | "designer">("runtime");

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

    async function handleOpenDesigner() {
        if (!schema) return;

        try {
            setError(null);

            const latestSchema = await getFormSchema(schema.formVersionId);

            if (latestSchema.status !== "Draft") {
                setError("Only Draft form versions can be edited.");
                return;
            }

            setSchema(latestSchema);
            setActivePage("designer");
        } catch (err) {
            setError(
                err instanceof Error ? err.message : "Unknown error"
            );
        }
    }

    async function handleOpenRuntime() {
        if (!schema) return;

        try {
            setError(null);

            const latestSchema = await getFormSchema(schema.formVersionId);

            setSchema(latestSchema);
            setActivePage("runtime");
        } catch (err) {
            setError(
                err instanceof Error ? err.message : "Unknown error"
            );
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
            <>
                <nav className="page-navigation">
                    <button
                        onClick={handleOpenRuntime}
                        disabled={activePage === "runtime"}
                    >
                        Runtime / Test
                    </button>

                    <button
                        onClick={handleOpenDesigner}
                        disabled={activePage === "designer"}
                    >
                        Form Designer
                    </button>
                </nav>

                {activePage === "runtime" ? (
                    <FormRuntimePage schema={schema} />
                ) : (
                    <FormDesignerPage schema = {schema} />
                )}
            </>
        )}
    </div>
);
}

export default App;