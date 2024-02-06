CREATE TABLE public.usuario (
	usua_cd_usuario varchar NOT NULL,
	usua_json_data jsonb NULL,
	usua_tx_nome varchar NULL,
	usua_dt_criacao timestamptz NULL,
	usua_cd_criado_por varchar NULL,
	CONSTRAINT usuario_pk PRIMARY KEY (usua_cd_usuario)
);
CREATE INDEX usuario_usua_cd_usuario_idx ON public.usuario USING btree (usua_cd_usuario);


-- public.livro definition

-- Drop table

-- DROP TABLE public.livro;

CREATE TABLE public.livro (
	livr_cd_livro varchar NOT NULL,
	livr_tx_titulo varchar NULL,
	livr_dt_criacao timestamptz NULL,
	usua_cd_criado_por varchar NULL,
	CONSTRAINT livro_pk PRIMARY KEY (livr_cd_livro)
);