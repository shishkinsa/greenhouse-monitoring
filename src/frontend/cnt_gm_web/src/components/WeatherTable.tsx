import React from 'react';
import { Table } from 'antd';
import type { ColumnsType } from 'antd/es/table';

interface WeatherData {
  key?: string;
  date: string;
  temperatureC: number;
  temperatureF?: number;
  summary: string;
}

interface WeatherTableProps {
  data: WeatherData[];
  loading?: boolean;
}

const WeatherTable: React.FC<WeatherTableProps> = ({ data, loading = false }) => {
  // Добавляем уникальный ключ для каждой строки, если его нет
  const dataSource = data.map((item, index) => ({
    ...item,
    key: item.key || index.toString(),
  }));

  const columns: ColumnsType<WeatherData> = [
    {
      title: 'Дата',
      dataIndex: 'date',
      key: 'date',
      sorter: (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime(),
    },
    {
      title: 'Температура (°C)',
      dataIndex: 'temperatureC',
      key: 'temperatureC',
      sorter: (a, b) => a.temperatureC - b.temperatureC,
    },
    {
      title: 'Описание',
      dataIndex: 'summary',
      key: 'summary',
    },
  ];

  return (
    <Table
      columns={columns}
      dataSource={dataSource}
      loading={loading}
      rowKey="key"
      pagination={{ pageSize: 5, showSizeChanger: true, showTotal: (total) => `Всего ${total} записей` }}
      bordered
      scroll={{ x: 'max-content' }}
    />
  );
};

export default WeatherTable;