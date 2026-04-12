import React from "react";

interface ButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  isLoading?: boolean
}

const Button: React.FC<ButtonProps> = ({onClick, children, isLoading = false}) => {
    return (
      <button onClick={onClick} disabled={isLoading}>
        {isLoading ? 'Загрузка...' : children}
      </button>
    );
};

export default Button;