import type { ColumnsType } from 'antd/es/table';
import { Table } from 'antd';
import type { WeatherForecast } from '../model/types';

/** Пропсы таблицы прогноза: данные и индикатор загрузки Ant Design `Table`. */
export interface WeatherTableProps {
  data: WeatherForecast[];
  loading?: boolean;
}

/**
 * Таблица Ant Design для отображения списка прогнозов (`WeatherForecast`).
 *
 * @param props.data — строки для отображения
 * @param props.loading — показывать ли спиннер загрузки над таблицей
 */
export function WeatherTable({ data, loading = false }: WeatherTableProps) {
  const dataSource = data.map((item, index) => ({
    ...item,
    key: item.key ?? index.toString(),
  }));

  const columns: ColumnsType<WeatherForecast> = [
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
      pagination={{
        pageSize: 5,
        showSizeChanger: true,
        showTotal: (total) => `Всего ${total} записей`,
      }}
      bordered
      scroll={{ x: 'max-content' }}
    />
  );
}
