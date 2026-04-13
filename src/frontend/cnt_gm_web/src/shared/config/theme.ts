import type { ThemeConfig } from 'antd';

/** Токены и настройки компонентов Ant Design для `ConfigProvider`. */
const theme: ThemeConfig = {
  token: {
    colorPrimary: '#1890ff',
    borderRadius: 6,
    fontSize: 14,
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
    Menu: {
      darkItemColor: 'rgba(255, 255, 255, 0.92)',
      darkItemHoverBg: 'rgba(255, 255, 255, 0.12)',
      /* Заметнее базового текста, чтобы hover читался на тёмном Sider */
      darkItemHoverColor: '#91caff',
      darkItemSelectedBg: '#1677ff',
      darkItemSelectedColor: '#ffffff',
    },
  },
};

export default theme;
