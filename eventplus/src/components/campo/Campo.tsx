import type { InputHTMLAttributes } from 'react';
import './Campo.css'

interface CampoProps extends InputHTMLAttributes<HTMLInputElement> {
    className?: string;
}

function Campo({
    placeholder,
    type = "text",
    className = "",
    ...props
}: CampoProps) {
    const classes = ["campo", className].filter(Boolean).join(" ")

    return(
        <input
        className={classes} 
        type={type} 
        placeholder={placeholder || "Digite algo..."}
        {...props}/>
    );
}

export default Campo;