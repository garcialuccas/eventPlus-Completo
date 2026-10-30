import { axiosInstance } from "./api";

import { type instituicao, type Comentario, type ComentarioPayload, type Evento, type EventoPayload, type LoginPayload, type LoginResponse, type MensagemModeracao, type Presenca, type PresencaPayload, type TipoUsuario, type Usuario, type UsuarioPayload, type TipoEvento } from "../types/api";

function eventoFormData(payload: EventoPayload){
    const formData = new FormData();
    formData.append("Nome", payload.nome);
    formData.append("Descricao", payload.descricao);
    formData.append("DataEvento", payload.dataEvento);
    formData.append("IdTipoEvento", payload.idTipoEvento || "");
    formData.append("IdInstituicao", payload.idInstituicao || "");
    if(payload.arquivoImagem) formData.append("ArquivoImagem", payload.arquivoImagem);
    if(payload.imagemUrl) formData.append("ImagemUrl", payload.imagemUrl);
    return formData
}

function progressoUpload(onProgress?: (porcentagem: number) => void){
    return {
        onUploadProgress: (progress: {loaded: number; total?: number }) => {
            if( progress.total && onProgress) {
                onProgress(Math.round((progress.loaded * 100) / progress.total))
            }
        }
    }
}

export const authService = {
    async login(payload: LoginPayload) {
        const { data } = await axiosInstance.post<LoginResponse>("Login", payload);
        return data;
    }
}

export const eventoService = {

    async listar(){
        const { data } = await axiosInstance.get<Evento[]>("Evento")
        return data;
    },

    async listarProximos() {
        const { data } = await axiosInstance.get<Evento[]>("Evento/proximos");
        return data;
    },

    async buscar(id: string){
        const { data } = await axiosInstance.get<Evento>(`Evento/${id}`);
        return data;
    },

    async cadastrar(payload: EventoPayload, onProgress?: (percentage: number) => void) {
        const { data } = await axiosInstance.post<Evento>(
            "Evento", 
            eventoFormData(payload),
            progressoUpload(onProgress)
        );
        return data;
    },

    async atualizar(id: string, payload: EventoPayload, onProgress?: (percentage: number) => void){
        await axiosInstance.put(
            `Evento/${id}`,
            eventoFormData(payload),
            progressoUpload(onProgress)
        );
    },

    async excluir(id:string) {
        await axiosInstance.delete(`Evento/${id}`)
    }
}

export const comentarioService = {
    async listarPorEvento(idEvento: string){
        const { data } = await axiosInstance.get<Comentario[]>(`Comentario/evento/${idEvento}`);
        return data
    },

    async cadastrar(payload: ComentarioPayload){
        const { data } = await axiosInstance.post<Comentario | MensagemModeracao>(
            "Comentario",
            payload
        );
        return data;
    }
}

export const presencaService = {
    async listarPorUsuario(idUsuario: string){
        const { data } = await axiosInstance.get<Presenca[]>(`Presenca/usuario/${idUsuario}`);
        return data;
    },

    async cadastrar(payload: PresencaPayload) {
        const { data } = await axiosInstance.post<Presenca>("/Presenca", payload);
        return data;
    },

    async excluir(id: string){
        await axiosInstance.delete(`Presenca/${id}`);
    }
}

export const usuarioService = {

    async listar(){
        const { data } = await axiosInstance.get<Usuario[]>("Usuario");
        return data;
    },

    async buscar(id:string){
        const { data } = await axiosInstance.get<Usuario>(`Usuario/${id}`);
        return data;
    },

    async cadastrar(payload: UsuarioPayload){
        const { data } = await axiosInstance.post<Usuario>("Usuario", payload);
        return data;
    },

    async atualizar(id: string, payload: UsuarioPayload){
        await axiosInstance.put(`Usuario/${id}`, payload);
    },

    async excluir(id: string){
        await axiosInstance.delete(`Usuario/${id}`);
    }
}

function tipoService<T extends { titulo: string}>(endpoint:string){
    return {
        async listar(){
            const { data } = await axiosInstance.get<T[]>(endpoint);
            return data;
        },
        async cadastrar(titulo: string){
            const { data } = await axiosInstance.post<T>(endpoint, { titulo });
            return data;
        },
        async atualizar(id: string, titulo: string){
            await axiosInstance.put(`${endpoint}/${id}`, {titulo});
        },
        async excluir(id: string){
            await axiosInstance.delete(`${endpoint}/${id}`)
        }
    }
}

export const tipoUsuarioService = tipoService<TipoUsuario>("TipoUsuario");

export const tipoEventoService = tipoService<TipoEvento>("TipoEvento");

export const instituicaoService = {
    async listar() {
        const { data } = await axiosInstance.get<instituicao[]>("Instituicao");
        return data;
    }
}