import React from "react";

interface WeatherData {
  date: string;
  temperatureC: number;
  summary: string;
}

interface TableProps {
  data: WeatherData[];
}

const Table: React.FC<TableProps> = ({data}) => {
    if(data.length === 0) {
      return <p>Нет данных для отображения. Нажмите кнопку "Загрузить данные".</p>
    }

    return (
        <table cellPadding={8} cellSpacing={0} style={{border: '1px solid black', marginTop: '20px', width: '100%'}}>
          <thead>
            <tr>
              <th>Дата</th>
              <th>Температура (°C)</th>
              <th>Описание</th>
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={index}>
                <td>{item.date}</td>
                <td>{item.temperatureC}</td>
                <td>{item.summary}</td>
              </tr>
            ))}
          </tbody>
        </table>
    );
};

export default Table;