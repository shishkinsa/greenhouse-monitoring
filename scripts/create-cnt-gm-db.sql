-- PostgreSQL bootstrap script for CNT_GM_DB.
-- Run as a superuser (for example: postgres).

DO
$$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'cnt_gm_db_user') THEN
        CREATE ROLE cnt_gm_db_user
            WITH LOGIN
            PASSWORD 'ChangeMe_StrongPassword_123!';
    END IF;
END
$$;

DO
$$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'cnt_gm_db') THEN
        CREATE DATABASE cnt_gm_db
            WITH OWNER = cnt_gm_db_user
                 ENCODING = 'UTF8'
                 TEMPLATE = template0;
    END IF;
END
$$;

GRANT ALL PRIVILEGES ON DATABASE cnt_gm_db TO cnt_gm_db_user;
