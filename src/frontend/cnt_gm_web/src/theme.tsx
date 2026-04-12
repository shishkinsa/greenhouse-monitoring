import type { ThemeConfig } from 'antd';

const theme: ThemeConfig = {
  token: {
    colorPrimary: '#1890ff',      // основной цвет бренда
    borderRadius: 6,               // скругление углов
    fontSize: 14,                  // базовый размер шрифта
  },
  components: {
    Button: {
      colorPrimary: '#1890ff',
      borderRadius: 4,
    },
    Table: {
      headerBg: '#fafafa',
      rowHoverBg: '#f5f5f5',
    },
  },
};

export default theme;