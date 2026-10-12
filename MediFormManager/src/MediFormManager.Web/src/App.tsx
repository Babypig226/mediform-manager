
import { useEffect, useState } from "react";
import type { FormSchema } from "./types/formSchema";
import { getFormSchema } from "./services/formSchemaApi";
import { login } from "./services/authApi";
import FormRuntimePage from "./pages/FormRuntimePage";
import FormDesignerPage from "./pages/FormDesignerPage";
import "./App.css";

type PageMode = "runtime" | "designer";

interface HostMessage {
    type: string;
    accessToken?: string;
    formVersionId?: string;
    mode?: string;
}

interface WebViewBridge {
    addEventListener(
        type: "message",
        listener: (event: MessageEvent<HostMessage>) => void
    ): void;
    removeEventListener(
        type: "message",
        listener: (event: MessageEvent<HostMessage>) => void
    ): void;
    postMessage(message: object): void;
}

function getWebViewBridge(): WebViewBridge | undefined {
    const hostWindow = window as Window & {
        chrome?: {
            webview?: WebViewBridge;
        };
    };

    return hostWindow.chrome?.webview;
}

function getLaunchContext() {
    const params = new URLSearchParams(window.location.search);

    return {
        formVersionId: params.get("formVersionId"),
        mode: (params.get("mode") === "designer"
            ? "designer"
            : "runtime") as PageMode
    };
}

function App() {
    const [launchContext] = useState(getLaunchContext);
    const [bridge] = useState(getWebViewBridge);

    const isHosted = Boolean(bridge);

    const [loginId, setLoginId] = useState("");
    const [password, setPassword] = useState("");

    const [schema, setSchema] = useState<FormSchema | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const [activePage, setActivePage] = useState<PageMode>(
        launchContext.mode
    );

    useEffect(() => {
        if (!bridge)
            return;

        let active = true;

        async function handleHostMessage(
            event: MessageEvent<HostMessage>
        ) {
            const message = event.data;

            if (message?.type !== "MEDIFORM_INIT")
                return;

            if (
                !message.accessToken ||
                message.formVersionId !== launchContext.formVersionId ||
                message.mode !== launchContext.mode
            ) {
                if (active)
                    setError("Invalid WPF launch context.");
                return;
            }

            try {
                setIsLoading(true);
                setError(null);

                sessionStorage.setItem(
                    "accessToken",
                    message.accessToken
                );

                const loadedSchema = await getFormSchema(
                    message.formVersionId
                );

                if (
                    message.mode === "designer" &&
                    loadedSchema.status !== "Draft"
                ) {
                    throw new Error(
                        "Only Draft form versions can be edited."
                    );
                }

                if (active) {
                    setSchema(loadedSchema);
                    setActivePage(message.mode as PageMode);
                }
            } catch (err) {
                if (active) {
                    setError(
                        err instanceof Error
                            ? err.message
                            : "Failed to initialize form."
                    );
                }
            } finally {
                if (active)
                    setIsLoading(false);
            }
        }

        bridge.addEventListener("message", handleHostMessage);

        bridge.postMessage({
            type: "MEDIFORM_READY"
        });

        return () => {
            active = false;
            bridge.removeEventListener(
                "message",
                handleHostMessage
            );
        };
    }, [bridge, launchContext.formVersionId, launchContext.mode]);

    async function handleLogin() {
        if (!launchContext.formVersionId) {
            setError("Please select a form version.");
            return;
        }

        try {
            setIsLoading(true);
            setError(null);

            const result = await login({ loginId, password });

            sessionStorage.setItem(
                "accessToken",
                result.accessToken
            );

            const loadedSchema = await getFormSchema(
                launchContext.formVersionId
            );

            if (
                launchContext.mode === "designer" &&
                loadedSchema.status !== "Draft"
            ) {
                throw new Error(
                    "Only Draft form versions can be edited."
                );
            }

            setSchema(loadedSchema);
            setActivePage(launchContext.mode);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Login failed."
            );
        } finally {
            setIsLoading(false);
        }
    }

    async function handleOpenDesigner() {
        if (!schema) return;

        try {
            setError(null);

            const latestSchema = await getFormSchema(
                schema.formVersionId
            );

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

            const latestSchema = await getFormSchema(
                schema.formVersionId
            );

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

            {!launchContext.formVersionId ? (
                <p role="status">
                    Select a form version from Forms Management.
                </p>
            ) : (
                <>
                    {!isHosted && !schema && (
                        <div>
                            <input
                                type="text"
                                placeholder="Login ID"
                                value={loginId}
                                onChange={e => setLoginId(e.target.value)}
                            />

                            <input
                                type="password"
                                placeholder="Password"
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                            />

                            <button
                                type="button"
                                onClick={handleLogin}
                                disabled={isLoading}
                            >
                                Login
                            </button>
                        </div>
                    )}

                    {isHosted && !schema && !error && (
                        <p>Connecting to WPF session...</p>
                    )}

                    {isLoading && <p>Loading...</p>}

                    {error && <p role="alert">{error}</p>}

                    {schema && (
                        <>
                            {!isHosted && (
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
                            )}

                            {activePage === "runtime" ? (
                                <FormRuntimePage
                                    key={schema.formVersionId}
                                    schema={schema}
                                />
                            ) : (
                                <FormDesignerPage
                                    key={schema.formVersionId}
                                    schema={schema}
                                />
                            )}
                        </>
                    )}
                </>
            )}
        </div>
    );
}

export default App;
