CREATE TABLE IF NOT EXISTS role
(
    id         UUID PRIMARY KEY,
    name       TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    deleted_at TIMESTAMP
);

CREATE TABLE IF NOT EXISTS "user"
(
    auth_server_id UUID PRIMARY KEY,
    created_at     TIMESTAMP DEFAULT NOW(),
    deleted_at     TIMESTAMP,
    role_id        UUID REFERENCES role (id)
);

CREATE TABLE IF NOT EXISTS "group"
(
    id         UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name       TEXT                                    NOT NULL,
    created_at TIMESTAMP        DEFAULT NOW(),
    deleted_at TIMESTAMP,
    created_by UUID REFERENCES "user" (auth_server_id) NOT NULL,
    admin      UUID REFERENCES "user" (auth_server_id) NOT NULL
);

CREATE TABLE IF NOT EXISTS user_group
(
    user_id  UUID REFERENCES "user" (auth_server_id),
    group_id UUID REFERENCES "group" (id),
    PRIMARY KEY (user_id, group_id)
);

CREATE TABLE IF NOT EXISTS todo
(
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title       TEXT                                    NOT NULL,
    description TEXT                                    NOT NULL,
    completed   BOOLEAN          DEFAULT FALSE,
    limit_date  TIMESTAMP,
    created_at  TIMESTAMP        DEFAULT NOW(),
    deleted_at  TIMESTAMP,
    created_by  UUID REFERENCES "user" (auth_server_id) NOT NULL,
    in_group    UUID REFERENCES "group" (id)
);

INSERT INTO role(id, name)
VALUES ('a4936970-4a99-40c7-9944-a9c61c9101d5', 'default'),
       ('67b8118c-7d18-4180-bd58-0730f134fe71', 'admin');
