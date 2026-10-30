import { Navigate, Route, Routes } from "react-router-dom";

import Home from "../pages/Home/Home"
import Eventos from "../pages/Eventos/Eventos"
import DetalhesEvento from "../pages/DetalhesEvento/DetalhesEvento";
import Login from "../pages/Login/Login";
import Usuarios from "../pages/Usuarios/Usuarios";
import CadastroEvento from "../pages/CadastroEvento/CadastroEvento";

import CadastroTipoEvento from "../pages/CadastroTipoEvento/CadastroTipoEvento";
import CadastroTipoUsuario from "../pages/CadastroTipoUsuario/CadastroTipoUsuario";

function AppRoutes() {
    return(
        <Routes>
            <Route path="/home" element={<Home/>} />

            <Route path="/eventos" element={<Eventos/>} />

            <Route path="/eventos/:id" element={<DetalhesEvento/>}/>

            <Route path="/login" element={<Login/>}/>

            <Route path="/usuarios" element={<Usuarios/>}/>

            <Route path="/cadastroEvento" element={<CadastroEvento/>}/>

            <Route path="/cadastroTipoEvento" element={<CadastroTipoEvento/>}/>

            <Route path="/cadastroTipoUsuario" element={<CadastroTipoUsuario/>}/>

            <Route path="*" element={<Navigate to="/home" replace/>}/>
        </Routes>
    )
}

export default AppRoutes;