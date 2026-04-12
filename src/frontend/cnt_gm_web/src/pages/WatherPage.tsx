import React, { useState } from "react";
import { Button } from 'antd';
import WeatherTable from '../components/WeatherTable';

const WatherPage: React.FC = () => {
  const [weatherData, setWeatherData] = useState([]);
  const [loading, setLoading] = useState(false);

  const fetchWeather = async () => {
    setLoading(true);
    try {
      const response = await fetch('/api/weatherforecast/');
      if (!response.ok) {
        throw new Error('Ошибка загрузки данных');
      }
      const data = await response.json();
      setWeatherData(data);
    }
    catch (error) {
      console.error('ОшибкаЖ', error);
      alert('Не удалось загрузить данные!');
    }
    finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Прогноз погоды</h1>
      <Button
        type="primary"
        onClick={fetchWeather}
        loading={loading}
      >
        {loading ? 'Загрузка...' : 'Загрузить данные'}
      </Button>
      <br/>
      <WeatherTable  data={weatherData} loading={loading}/>
    </div>
  );
};

export default WatherPage;