
CREATE TABLE [DotNetCoreLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [int] NULL,
	[Level] [varchar](200) NULL,
	[Url] [varchar](100) NULL,
	[Logger] [varchar](500) NULL,
	[Action] [varchar](100) NULL,
	[SourceAction] [varchar](100) NULL,
	[Message] [varchar](8000) NULL,
	[Exception] [varchar](4000) NULL,
	[App] [varchar](100) NULL,
	[User] [varchar](100) NULL,
	[RequestBody] [varchar](8000) NULL,
	[Reference] [varchar](100) NULL,
	[Json] [varchar](4000) NULL
) ON [PRIMARY]
GO

CREATE PROCEDURE [SP_InsertLog]	
(
	@LogId int output,
	@Date datetime,
	@Thread int,
	@Level varchar(200),
	@Url varchar(100),
	@Logger varchar(500),
	@Action varchar(100),
	@SourceAction varchar(100),
	@Message varchar(8000),
	@Exception varchar(4000),
	@App varchar(100) output,
	@User varchar(100),
	@RequestBody varchar(8000),
	@Reference varchar(100)=null,
	@Json varchar(4000) = null
	)
AS BEGIN
    SET NOCOUNT ON;
		
	INSERT INTO DotNetCoreLog
          (  
			[Date],
			Thread,
			[Level],
			[Url],
			Logger,
			[Action],
			SourceAction,
			[Message],
			Exception,
			[App],
			[User],
			RequestBody,
			Reference,
			[Json]
          ) 
     VALUES 
          ( 
			@Date,
			@Thread,
			@Level,
			@Url,
			@Logger,
			@Action,
			@SourceAction,
			@Message,
			@Exception,
			@App,
			@User,
			@RequestBody,
			@Reference,
			@Json
          );	
		  	SELECT @LogId = @@IDENTITY , @App='DotNetCoreRepository'
END
GO


CREATE PROCEDURE [SP_UpdateLog]	
	@LogId int,
	@Date datetime,
	@Thread int,
	@Level varchar(200),
	@Url varchar(100),
	@Logger varchar(500),
	@Action varchar(100),
	@SourceAction varchar(100),
	@Message varchar(8000),
	@Exception varchar(4000),
	@App varchar(100),
	@User varchar(100),
	@RequestBody varchar(8000),
	@Reference varchar(100)=null,
	@Json varchar(4000) = null
AS BEGIN
    SET NOCOUNT ON;
		
	update DotNetCoreLog
	set 
	[Date] = @Date,
	[Thread] = @Thread,
	[Level] = @Level,
	[Url] = @Url,
	Logger = @Logger,
	[Action] =@Action,
	SourceAction = @SourceAction,
	[Message] = @Message,
	Exception = @Exception,
	[App] = @App,
	[User] = @User,
	RequestBody = @RequestBody,
	Reference = @Reference,
	[Json] = @Json
     where LogId= @LogId
END

GO

CREATE PROCEDURE [SP_UpdateLogStatus]	
	@LogId int,
	@StatusId int
AS BEGIN
    SET NOCOUNT ON;
		
	update DotNetCoreLog
	set 
	StatusId=@StatusId
     where LogId= @LogId
END

GO


CREATE PROCEDURE [SP_DeleteLog]	
	@LogId int
AS BEGIN
    SET NOCOUNT ON;
	delete from DotNetCoreLog
     where LogId= @LogId
END

GO


create PROCEDURE [SP_GetLogs]
	@LogId int=null
AS BEGIN
    select * from DotNetCoreLog
where ((@LogId is null) or (LogId= @LogId)) 
END

