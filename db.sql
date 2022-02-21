-- DB MIGRATIONS FILE
-- DON'T CHANGE, ONLY APPEND

CREATE EXTENSION "uuid-ossp";

CREATE ROLE dev;
GRANT CONNECT ON DATABASE livelines TO dev;

CREATE TABLE users
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    provider TEXT NOT NULL,
    username TEXT UNIQUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    last_login TIMESTAMP WITH TIME ZONE NULL DEFAULT NULL
);

CREATE INDEX users_username_idx ON users (username);

GRANT UPDATE, INSERT, SELECT ON TABLE users TO dev;

CREATE TABLE lines
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID REFERENCES users (id) NOT NULL,
    body TEXT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

GRANT UPDATE, INSERT, SELECT ON TABLE lines TO dev;

-- v1: add song table and song id to lines
BEGIN;
    CREATE TABLE songs
    (
        id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
        spotify_id TEXT UNIQUE NOT NULL
    );

    CREATE INDEX songs_spotify_id_idx ON songs (spotify_id);

    GRANT UPDATE, INSERT, SELECT ON TABLE songs TO dev;

    ALTER TABLE lines
    ADD COLUMN song_id UUID REFERENCES songs (id) NULL;
COMMIT;
-- end

-- v2: add date_for column to lines
BEGIN;
    ALTER TABLE lines
    ADD COLUMN date_for DATE NULL;

    UPDATE lines
    SET date_for = DATE(created_at);

    ALTER TABLE lines
    ALTER COLUMN date_for SET NOT NULL;
COMMIT;
-- end 

-- v3: add privacy to lines and create new profiles table
BEGIN;
    CREATE TYPE PRIVACY AS ENUM ('Private', 'Unlisted', 'Public');

    ALTER TABLE lines
    ADD COLUMN privacy PRIVACY NOT NULL DEFAULT 'Private';

    CREATE TABLE profiles
    (
        id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
        user_id UUID REFERENCES users (id) UNIQUE NOT NULL,
        last_updated TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
        default_privacy PRIVACY NOT NULL DEFAULT 'Private'
    );
    
    CREATE INDEX profiles_user_id_idx ON profiles (user_id);
    
    GRANT UPDATE, INSERT, SELECT ON TABLE profiles TO dev;
COMMIT;
-- end
