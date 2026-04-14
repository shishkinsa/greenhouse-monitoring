import { TableOutlined, HomeOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Breadcrumb, Layout, Menu, theme } from 'antd';
import { useState, type ReactNode } from 'react';
import { Link, Outlet, useLocation } from 'react-router-dom';
import styles from './AppLayout.module.css';

const { Header, Content, Footer, Sider } = Layout;

type MenuItem = Required<MenuProps>['items'][number];

function getItem(
  label: ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[],
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
  } as MenuItem;
}

const items: MenuItem[] = [
  getItem(<Link to="/home">Главная</Link>, '/home', <HomeOutlined />),
  getItem(<Link to="/weather">Прогноз погоды</Link>, '/weather', <TableOutlined />),
];

/**
 * Оболочка приложения: боковое меню, шапка, область для вложенных маршрутов (`Outlet`).
 */
export function AppLayout() {
  const [collapsed, setCollapsed] = useState(false);
  const { pathname } = useLocation();
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const menuSelectedKeys = items.some((item) => item && 'key' in item && item.key === pathname)
    ? [pathname]
    : [];

  return (
    <Layout className={styles.rootLayout}>
      <Sider
        collapsible
        collapsed={collapsed}
        onCollapse={setCollapsed}
        breakpoint="lg"
        collapsedWidth={0}
        zeroWidthTriggerStyle={{ top: 16 }}
      >
        <div className={styles.demoLogoVertical} />
        <Menu theme="dark" mode="inline" items={items} selectedKeys={menuSelectedKeys} />
      </Sider>
      <Layout className={styles.innerLayout}>
        <Header style={{ padding: 0, background: colorBgContainer }} />
        <Content
          style={{
            margin: '0 16px',
            background: colorBgContainer,
            borderRadius: borderRadiusLG,
            display: 'flex',
            flexDirection: 'column',
            minHeight: 0,
            flex: 1,
          }}
        >
          <Breadcrumb style={{ margin: '16px 0', flexShrink: 0 }} items={[{ title: 'User' }, { title: 'Bill' }]} />
          <div className={styles.contentContainer}>
            <Outlet />
          </div>
        </Content>
        <Footer style={{ textAlign: 'center' }}>
          Greenhouse Monitoring ©{new Date().getFullYear()} Created by OrdoFlux Systems
        </Footer>
      </Layout>
    </Layout>
  );
}
