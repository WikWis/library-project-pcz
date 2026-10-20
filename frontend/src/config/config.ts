const BACKEND_URL = import.meta.env.PUBLIC_BACKEND_URL;

if (!BACKEND_URL) {
    throw new Error('Brak wymaganej zmiennej PUBLIC_BACKEND_URL');
}

export interface Config {
    public: {
        backendUrl: string
    }
}

export const config: Config = {
    public: {
        backendUrl: BACKEND_URL.replace(/\/+$/, ''),
    },
} as const;