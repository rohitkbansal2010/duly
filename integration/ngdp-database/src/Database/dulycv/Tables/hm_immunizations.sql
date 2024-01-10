CREATE TABLE [dulycv].[hm_immunizations] (
    [pat_id]           VARCHAR (18)  NOT NULL,
    [line]             INT           NOT NULL,
    [hm_topic_id]      NUMERIC (18)  NOT NULL,
    [vaccine_name]     VARCHAR (200) NULL,
    [ideal_return_dt]  DATETIME      NULL,
    [hmt_due_status_c] INT           NULL,
    [hm_due_status]    VARCHAR (254) NULL,
    [frequency]        VARCHAR (254) NULL
);

