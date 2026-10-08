import { useState } from "react";
import type { FormSchema } from "./types/formSchema";
import { getFormSchema } from "./services/formSchemaApi";
import { login } from "./services/authApi";
import "./App.css";
import FormRuntimePage from "./pages/FormRuntimePage";
import FormDesignerPage from "./pages/FormDesignerPage";

const FORM_VERSION_ID = "47571803-b7db-4c77-a2b6-61455dcf861c";

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
                        onClick={() => setActivePage("runtime")}
                        disabled={activePage === "runtime"}
                    >
                        Runtime / Test
                    </button>

                    <button
                        onClick={() => setActivePage("designer")}
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