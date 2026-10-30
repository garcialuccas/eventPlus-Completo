import { useCallback, useEffect, useState, type ChangeEvent, type FormEvent } from "react";
import Footer from "../../components/footer/Footer";
import Header from "../../components/header/Header";
import type { Usuario, TipoUsuario } from "../../types/api";
import { usuarioService, tipoUsuarioService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import Campo from "../../components/campo/Campo";
import "./Usuarios.css";

import fotoMulher from "../../assets/imagem_mulher_apresentando.png"

interface FormularioUsuario {
    nome: string;
    email: string;
    senha: string;
    idTipoUsuario: string;
}

const inicial: FormularioUsuario = {
    nome: "",
    email: "",
    senha: "",
    idTipoUsuario: ""
};

function CadastroUsuario() {
    const [formulario, setFormulario] = useState(inicial);
    const [tipos, setTipos] = useState<TipoUsuario[]>([]);
    const [usuarios, setUsuarios] = useState<Usuario[]>([]);
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [selecionado, setSelecionado] = useState<Usuario | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("");

    const carregar = useCallback(async () => {
        setCarregando(true);
        setMensagem("");
        try {
            const [tiposApi, usuariosApi] = await Promise.all([
                tipoUsuarioService.listar(),
                usuarioService.listar()
            ]);

            setTipos(tiposApi);
            setUsuarios(usuariosApi);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false);
        }
    }, []);

    useEffect(() => {
        const timeout = window.setTimeout(() => void carregar(), 0);
        return () => window.clearTimeout(timeout);
    }, [carregar]);

    function atualizar(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
        setFormulario((value) => ({ ...value, [event.target.name]: event.target.value }));
    }

    function limparFormulario() {
        setFormulario(inicial);
        setIdEmEdicao(null);
    }

    function editar(usuario: Usuario) {
        setIdEmEdicao(usuario.idUsuario);
        setMensagem("");
        setFormulario({
            nome: usuario.nome,
            email: usuario.email,
            senha: "",
            idTipoUsuario: usuario.idTipoUsuario
        });
        window.scrollTo({ top: 0, behavior: "smooth" });
    }

    function nomeDoTipo(idTipoUsuario: string) {
        return tipos.find((tipo) => tipo.idTipoUsuario === idTipoUsuario)?.titulo ?? "";
    }

    async function salvar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setEnviando(true);
        setMensagem("");

        try {
            const payload = {
                nome: formulario.nome.trim(),
                email: formulario.email.trim(),
                idTipoUsuario: formulario.idTipoUsuario,
                senha: formulario.senha
            };

            if (idEmEdicao) await usuarioService.atualizar(idEmEdicao, payload);
            else await usuarioService.cadastrar(payload);

            const mensagemSucesso = idEmEdicao ? "Usuário atualizado com sucesso." : "Usuário cadastrado com sucesso.";

            limparFormulario();
            setUsuarios(await usuarioService.listar());
            setMensagem(mensagemSucesso);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    async function excluir() {
        if (!selecionado) return;
        setEnviando(true);
        setMensagem("");

        try {
            await usuarioService.excluir(selecionado.idUsuario);
            if (idEmEdicao === selecionado.idUsuario) limparFormulario();
            setUsuarios(await usuarioService.listar());
            setMensagem("Usuário excluído com sucesso.");
            setSelecionado(null);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
            setSelecionado(null);
        } finally {
            setEnviando(false);
        }
    }

    return (
        <>
            <Header />
            <main className="cadastro-usuario-page">
                <section className="usuario-topo">
                    <h1 className="titulo-pagina">Administração Usuário</h1>

                    <div className="usuario-conteudo">
                        <div className="usuario-ilustracao">
                            <img src={fotoMulher} alt="Pessoa ao lado de um cadastro de usuário" />
                        </div>

                        <form className="usuario-form" onSubmit={salvar}>
                            <h2 className="visually-hidden">{idEmEdicao ? "Editar usuário" : "Cadastrar usuário"}</h2>

                            <div className="linha">
                                <label className="visually-hidden" htmlFor="usuario-nome">Nome</label>
                                <Campo
                                    id="usuario-nome"
                                    name="nome"
                                    value={formulario.nome}
                                    onChange={atualizar}
                                    placeholder="Nome"
                                    maxLength={100}
                                    autoComplete="off"
                                    required
                                />
                            </div>

                            <div className="linha">
                                <label className="visually-hidden" htmlFor="usuario-email">Email</label>
                                <Campo
                                    id="usuario-email"
                                    name="email"
                                    type="email"
                                    value={formulario.email}
                                    onChange={atualizar}
                                    placeholder="Email"
                                    maxLength={100}
                                    autoComplete="off"
                                    required
                                />
                            </div>

                            <div className="linha">
                                <div className="campo-select">
                                    <label className="visually-hidden" htmlFor="usuario-tipo">Tipo de usuário</label>
                                    <select
                                        id="usuario-tipo"
                                        name="idTipoUsuario"
                                        value={formulario.idTipoUsuario}
                                        onChange={atualizar}
                                        required
                                    >
                                        <option value="" disabled>Tipo de Usuário</option>
                                        {tipos.map((tipo) => (
                                            <option key={tipo.idTipoUsuario} value={tipo.idTipoUsuario}>
                                                {tipo.titulo}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div>
                                    <label className="visually-hidden" htmlFor="usuario-senha">Senha</label>
                                    <Campo
                                        id="usuario-senha"
                                        name="senha"
                                        type="password"
                                        value={formulario.senha}
                                        onChange={atualizar}
                                        placeholder="Senha"
                                        autoComplete="new-password"
                                        required
                                    />
                                </div>
                            </div>

                            {mensagem && <p className="mensagem" role="status">{mensagem}</p>}

                            <button className="botao-principal" type="submit" disabled={enviando || carregando}>
                                {enviando ? "Enviando..." : idEmEdicao ? "Atualizar" : "Cadastrar"}
                            </button>

                            {idEmEdicao && (
                                <button className="botao-secundario" type="button" onClick={limparFormulario} disabled={enviando}>
                                    Cancelar edição
                                </button>
                            )}
                        </form>
                    </div>
                </section>

                <section className="usuario-lista" aria-labelledby="titulo-lista-usuarios">
                    <h2 id="titulo-lista-usuarios" className="titulo-pagina">Lista Usuários</h2>

                    {carregando ? (
                        <p className="lista-vazia">Carregando usuários...</p>
                    ) : usuarios.length === 0 ? (
                        <p className="lista-vazia">Nenhum usuário cadastrado ainda. Preencha o formulário acima para criar o primeiro.</p>
                    ) : (
                        <div className="tabela-rolagem">
                            <table>
                                <thead>
                                    <tr>
                                        <th scope="col">Nome</th>
                                        <th scope="col">Email</th>
                                        <th scope="col">Tipo de Usuário</th>
                                        <th scope="col" className="coluna-acao">Editar</th>
                                        <th scope="col" className="coluna-acao">Deletar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {usuarios.map((usuario) => (
                                        <tr key={usuario.idUsuario}>
                                            <td>{usuario.nome}</td>
                                            <td>{usuario.email}</td>
                                            <td>{nomeDoTipo(usuario.idTipoUsuario)}</td>
                                            <td className="coluna-acao">
                                                <button
                                                    type="button"
                                                    className="icone-botao"
                                                    aria-label={`Editar ${usuario.nome}`}
                                                    onClick={() => editar(usuario)}
                                                    disabled={enviando}
                                                >
                                                    <svg viewBox="0 0 24 24" aria-hidden="true">
                                                        <path
                                                            fill="currentColor"
                                                            d="M3 17.25V21h3.75L17.8 9.94l-3.75-3.75L3 17.25zM20.7 7.04a1 1 0 0 0 0-1.41l-2.34-2.34a1 1 0 0 0-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"
                                                        />
                                                    </svg>
                                                </button>
                                            </td>
                                            <td className="coluna-acao">
                                                <button
                                                    type="button"
                                                    className="icone-botao"
                                                    aria-label={`Deletar ${usuario.nome}`}
                                                    onClick={() => setSelecionado(usuario)}
                                                    disabled={enviando}
                                                >
                                                    <svg viewBox="0 0 24 24" aria-hidden="true">
                                                        <path
                                                            fill="currentColor"
                                                            d="M6 19a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V8H6v11zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"
                                                        />
                                                    </svg>
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </section>

                {selecionado && (
                    <div className="modal-fundo" onClick={() => !enviando && setSelecionado(null)}>
                        <div
                            className="modal"
                            role="alertdialog"
                            aria-modal="true"
                            aria-labelledby="modal-titulo-usuario"
                            onClick={(e) => e.stopPropagation()}
                        >
                            <h2 id="modal-titulo-usuario">Excluir usuário?</h2>
                            <p>
                                O usuário <strong>{selecionado.nome}</strong> será excluído e essa ação não pode ser desfeita.
                            </p>
                            <div className="modal-acoes">
                                <button type="button" className="botao-secundario" onClick={() => setSelecionado(null)} disabled={enviando}>
                                    Cancelar
                                </button>
                                <button type="button" className="botao-principal" onClick={excluir} disabled={enviando}>
                                    {enviando ? "Excluindo..." : "Excluir"}
                                </button>
                            </div>
                        </div>
                    </div>
                )}
            </main>
            <Footer />
        </>
    );
}

export default CadastroUsuario;
