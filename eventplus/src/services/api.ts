import axios, { AxiosError } from "axios";
import type { AuthSession } from "../types/api";

export const SESSION_STORAGE_KEY = "eventplus:session"
export const UNAUTHORIZED_EVENT = "eventplus:unauthorized"

interface ValidationProblem {
    title?: string;
    detail?: string;
    errors?: Record<string, string[]>;
}

export class ApiError extends Error {
    status?: number;
    validationErrors?: Record<string, string[]>;

    constructor(
        message: string,
        status?: number,
        validationErrors?: Record<string, string[]>,
    ) {
        super(message);
        this.name = "ApiError";
        this.status = status;
        this.validationErrors = validationErrors;
    }
}

function readToken() {
    try {
        const value = localStorage.getItem(SESSION_STORAGE_KEY);
        if (!value) return null;
        return (JSON.parse(value) as AuthSession).token ?? null;
    } catch {
        return null;
    }
}

function errorMessage(error: AxiosError<unknown>) {
    if (error.code === "ECONNABORTED") {
        return "A API demorou para responder. Tente novamente.";
    }

    if (!error.response) {
        return "Não foi possível conectar à API. Verifique se o servidor está disponível"
    }

    const data = error.response.data;

    if (typeof data === "string" && data.trim()) return data;

    if (data && typeof data === "object") {
        const problem = data as ValidationProblem;
        if (problem.detail) return problem.detail;
        if (problem.title) return problem.title;
        if (problem.errors) {
            const primeiro = Object.values(problem.errors).flat()[0];
            if (primeiro) return primeiro;
        }
    }

    return `A solicitação falhou com status ${error.response.status}.`
}

export const axiosInstance = axios.create({
    baseURL: "https://localhost:7169/api/",
    timeout: 15000,
    headers: {
        Accept: "application/json"
    }
})

axiosInstance.interceptors.request.use((config) => {
    const token = readToken();
    if (token) config.headers.Authorization = `Bearer ${token}`
    return config;
})

axiosInstance.interceptors.response.use((response) =>
    response, (motivo: AxiosError<unknown>) => {
        const status = motivo.response?.status;
        const data = motivo.response?.data as ValidationProblem | undefined;

        if (status === 401) {
            localStorage.removeItem(SESSION_STORAGE_KEY);
            window.dispatchEvent(new Event(UNAUTHORIZED_EVENT));
        }
        return Promise.reject(new ApiError(errorMessage(motivo), status, data?.errors));
    }
)

export function getApiErrorMessage(error: unknown) {
    return error instanceof ApiError
        ? error.message
        : "Ocorreu um erro inesperado. Tente Novamente.";
}