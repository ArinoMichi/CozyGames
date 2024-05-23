USE [master]
GO
/****** Object:  Database [GAMES]    Script Date: 19/03/2024 00:18:18 ******/
CREATE DATABASE [GAMES]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GAMES', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GAMES.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GAMES_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GAMES_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GAMES] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GAMES].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GAMES] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GAMES] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GAMES] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GAMES] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GAMES] SET ARITHABORT OFF 
GO
ALTER DATABASE [GAMES] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [GAMES] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GAMES] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GAMES] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GAMES] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GAMES] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GAMES] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GAMES] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GAMES] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GAMES] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GAMES] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GAMES] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GAMES] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GAMES] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GAMES] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GAMES] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GAMES] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GAMES] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GAMES] SET  MULTI_USER 
GO
ALTER DATABASE [GAMES] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GAMES] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GAMES] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GAMES] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GAMES] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GAMES] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GAMES] SET QUERY_STORE = OFF
GO
USE [GAMES]
GO
/****** Object:  Table [dbo].[JUEGO]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JUEGO](
	[IDJUEGO] [int] NOT NULL,
	[TITULO] [nvarchar](150) NULL,
	[DESCRIPCION] [nvarchar](600) NULL,
	[IMAGENBANNER] [nvarchar](500) NULL,
	[IMAGENV] [nvarchar](500) NULL,
	[IMAGENH] [nvarchar](500) NULL,
	[ENLACE] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IDJUEGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO](
	[IDUSUARIO] [int] NOT NULL,
	[NOMBRE] [nvarchar](20) NULL,
	[EMAIL] [nvarchar](100) NULL,
	[FOTO] [nvarchar](100) NULL,
	[PASS] [varbinary](max) NULL,
	[PASSORIGEN] [nvarchar](100) NULL,
	[SALT] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IDUSUARIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RATING]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RATING](
	[IDRATING] [int] NOT NULL,
	[IDJUEGO] [int] NULL,
	[IDUSUARIO] [int] NULL,
	[NOTA] [decimal](5, 2) NULL,
	[TITULO] [varchar](200) NULL,
	[COMENTARIO] [varchar](600) NULL,
	[FECHA] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDRATING] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LIKE_COMENTARIO]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LIKE_COMENTARIO](
	[IDLIKE] [int] NOT NULL,
	[IDRATING] [int] NULL,
	[IDUSUARIO] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDLIKE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[V_USER_GAME_RATING]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_USER_GAME_RATING] AS
SELECT
    R.IDRATING,
    J.IDJUEGO AS ID_JUEGO,
    J.TITULO AS NOMBRE_JUEGO,
    J.IMAGENBANNER AS FOTO_JUEGO,
    U.IDUSUARIO AS ID_USUARIO,
    U.NOMBRE AS NOMBRE_USUARIO,
    U.FOTO AS FOTO_USUARIO,
    R.NOTA,
    R.TITULO,
    R.COMENTARIO,
    R.FECHA,
    COUNT(LC.IDLIKE) AS LIKES_TOTALES
FROM
    RATING R
JOIN JUEGO J ON R.IDJUEGO = J.IDJUEGO
JOIN USUARIO U ON R.IDUSUARIO = U.IDUSUARIO
LEFT JOIN LIKE_COMENTARIO LC ON R.IDRATING = LC.IDRATING
GROUP BY
    R.IDRATING, J.IDJUEGO, J.TITULO,J.IMAGENBANNER, U.IDUSUARIO, U.NOMBRE, U.FOTO, R.NOTA, R.TITULO, R.COMENTARIO, R.FECHA;
GO
/****** Object:  View [dbo].[JUEGODETALLES]    Script Date: 19/03/2024 00:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[JUEGODETALLES]
AS
SELECT
    J.IDJUEGO,
    J.TITULO,
    J.DESCRIPCION,
    J.IMAGENBANNER,
    J.IMAGENV,
    J.IMAGENH,
    J.ENLACE,
    COALESCE(ROUND(AVG(R.NOTA), 2), 0) AS NOTA_PROMEDIO
FROM
    dbo.JUEGO AS J
LEFT OUTER JOIN
    dbo.RATING AS R ON J.IDJUEGO = R.IDJUEGO
GROUP BY
    J.IDJUEGO,
    J.TITULO,
    J.DESCRIPCION,
    J.IMAGENBANNER,
    J.IMAGENV,
    J.IMAGENH,
    J.ENLACE;
GO
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (1, N'Sticky Business', N'Experience the joy of running your own cozy small business: Create stickers, pack orders and hear your customers’ stories. Time to build the cutest shop on the internet!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2303350/header.jpg?t=1706288146', N'https://howlongtobeat.com/GAMES/129859_Sticky_Business.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2303350/ss_39fe1195b9064440ce19130df5d2e9f042456475.1920x1080.jpg?t=1706288146', N'https://store.steampowered.com/app/2303350/Sticky_Business/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (2, N'Stardew Valley', N'You''ve inherited your grandfather''s old farm plot in Stardew Valley. Armed with hand-me-down tools and a few coins, you set out to begin your new life. Can you learn to live off the land and turn these overgrown fields into a thriving home?', N'https://cdn.cloudflare.steamstatic.com/steam/apps/413150/header.jpg?t=1666917466', N'https://w0.peakpx.com/wallpaper/891/516/HD-wallpaper-stardew-valley-colors-game-GAMES-jogos-pixel-prybz.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/413150/ss_b887651a93b0525739049eb4194f633de2df75be.1920x1080.jpg?t=1666917466', N'https://store.steampowered.com/app/413150/Stardew_Valley/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (3, N'Coffee Talk', N'Coffee Talk is a coffee brewing and heart-to-heart talking simulator about listening to fantasy-inspired modern peoples’ problems, and helping them by serving up a warm drink or two.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/914800/header.jpg?t=1704255891', N'https://www.metacritic.com/a/img/catalog/provider/6/12/6-1-848746-52.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/914800/ss_fb39356fdb31c360f81598801f6398fcf83a6324.1920x1080.jpg?t=1704255891', N'https://store.steampowered.com/app/914800/Coffee_Talk/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (4, N'Turnip Boy Commits Tax Evasion', N'Play as an adorable yet trouble-making turnip. Avoid paying taxes, solve plantastic puzzles, harvest crops and battle massive beasts all in a journey to tear down a corrupt vegetable government!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1205450/header.jpg?t=1705692327', N'https://howlongtobeat.com/GAMES/80383_Turnip_Boy_Commits_Tax_Evasion.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1205450/ss_d92fbab485ac5314981052c03620a339dc22f21a.1920x1080.jpg?t=1705692327', N'https://store.steampowered.com/app/1205450/Turnip_Boy_Commits_Tax_Evasion/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (5, N'Cozy Grove', N'Welcome to Cozy Grove, a game about camping on a haunted, ever-changing island. As a Spirit Scout, you''ll wander the island''s forest each day, finding new hidden secrets and helping soothe the local ghosts. With a little time and a lot of crafting, you''ll bring color and joy back to Cozy Grove!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1458100/header.jpg?t=1678124698', N'https://static1.thegamerimages.com/wordpress/wp-content/uploads/2024/01/cozy-grove-cover.jpg?q=50&fit=crop&w=480&dpr=1.5', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1458100/ss_7ccc68d4ac9698f3066995d02587b50977dace71.1920x1080.jpg?t=1678124698', N'https://store.steampowered.com/app/1458100/Cozy_Grove/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (6, N'Good Pizza Great Pizza', N'Ever wanted to know what it feels like to run your own cozy Pizza shop? Do your best to fulfill pizza orders from customers while making enough money to keep your shop open. Upgrade your shop with new toppings and equipment to compete against your pizza rival, Alicante!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/770810/header.jpg?t=1703191580', N'https://howlongtobeat.com/GAMES/58422_Good_Pizza_Great_Pizza.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/770810/ss_6cb6ae88f7800a85d6ea307f0cca3bbcd686115d.1920x1080.jpg?t=1703191580', N'https://store.steampowered.com/app/770810/Good_Pizza_Great_Pizza__Cooking_Simulator_Game/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (7, N'Blanc', N'Blanc is an artistic cooperative adventure that follows the journey of a wolf cub and a fawn stranded in a vast, snowy wilderness. They must come together in an unlikely partnership to find their families.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1982340/header.jpg?t=1698419995', N'https://assets.nintendo.com/image/upload/f_auto/q_auto/dpr_1.0/c_scale,w_600/ncom/en_US/GAMES/switch/b/blanc-switch/description-image', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1982340/ss_9edcf9e1e0c0c0ac15e913d72fccb2029f59c9e6.1920x1080.jpg?t=1698419995', N'https://store.steampowered.com/app/1982340/Blanc/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (8, N'Melatonin', N'Melatonin is a rhythm game about dreams and reality merging together. It uses animations and sound cues to keep you on beat without any intimidating overlays or interfaces. Harmonize through a variety of dreamy levels containing surprising challenges, hand-drawn art, and vibrant music.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1585220/header.jpg?t=1705972187', N'https://images.g2a.com/360x600/1x1x1/melatonin-pc-steam-key-global-i10000338153001/39b3ca4ac7564f56ad945b9a', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1585220/ss_f24cf6fe64fc644e0cccc9c0b1b5f1a900199d56.1920x1080.jpg?t=1705972187', N'https://store.steampowered.com/app/1585220/Melatonin/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (9, N'A little to the left', N'A Little to the Left is a cozy puzzle game that has you sort, stack, and organize household items into pleasing arrangements while you keep an eye out for a mischievous cat with an inclination for chaos. Check out this playful and intuitive puzzler with 100+ satisfying messes to tidy.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1629520/header.jpg?t=1706716839', N'https://pirated-GAMES.net/wp-content/uploads/2022/11/a-little-to-the-leftfeatured_img_600x900.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1629520/ss_dfd3c87efd52db3ea48b4de22de569bd9eb42ca2.1920x1080.jpg?t=1706716839', N'https://store.steampowered.com/app/1629520/A_Little_to_the_Left/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (10, N'Everything', N'Be the Universe in this epic, award-winning reality simulation game - featuring thousands of playable characters, endless exploration, an extraordinary soundtrack and narration from Alan Watts.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/582270/header.jpg?t=1674789705', N'https://m.media-amazon.com/images/M/MV5BZTc1ZjVlYjQtYmJhNy00YTc0LWIzNTAtZjczMzMyM2U3MWU2XkEyXkFqcGdeQXVyMTA0MTM5NjI2._V1_.jpg', N'https://cdn2.unrealengine.com/Diesel%2Fproductv2%2Feverything%2Fhome%2FEGS_DavidOReilly_Everything_CAROUSEL_1-2560x1440-0e44fd03c3063cd4b95964c727335c195b516d32.jpg', N'https://store.steampowered.com/app/582270/Everything/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (11, N'Gris', N'Gris is a girl full of hope and lost in her own world, faced with a painful experience in her life.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/683320/header.jpg?t=1686261902', N'https://spainaudiovisualhub.mineco.gob.es/content/dam/seteleco-hub-audiovisual/resources/images/videojuegos/02_gris.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/683320/ss_a155ad5423e11e3e764a1a270dcf4f30323f0a35.1920x1080.jpg?t=1686261902', N'https://store.steampowered.com/app/683320/GRIS/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (12, N'Calico', N'Calico is a day-in-the-life community sim game where you are given an important and adorable task: rebuild the town’s cat café and fill it with cute and cuddly creatures! Build up your café by filling it with cute furniture, fun decorations, yummy pastries, and get it bustling with animals again!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1112890/header.jpg?t=1680722460', N'https://howlongtobeat.com/GAMES/85354_Calico.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1112890/ss_5b8222dc7e5f9bbcbdd7475062db1a2728023764.1920x1080.jpg?t=1680722460', N'https://store.steampowered.com/app/1112890/Calico/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (13, N'A Short Hike', N'Hike, climb, and soar through the peaceful mountainside landscapes of Hawk Peak Provincial Park as you make your way to the summit.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1055540/header.jpg?t=1701219411', N'https://pics.filmaffinity.com/A_Short_Hike-150817168-large.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1055540/ss_0e864bf975bb71f238de6861fc8fd3d6ed6e4ce8.1920x1080.jpg?t=1701219411', N'https://store.steampowered.com/app/1055540/A_Short_Hike/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (14, N'Night in the Woods', N'NIGHT IN THE WOODS is an adventure game focused on exploration, story, and character, featuring dozens of characters to meet and lots to do across a lush, vibrant world.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/481510/header.jpg?t=1706292417', N'https://store-images.s-microsoft.com/image/apps.34394.67507673447846593.6652438a-e13f-460b-afd9-a160c1f79ade.96769618-2073-4cf4-bee6-f02122a3d5f9?mode=scale&q=90&h=300&w=200', N'https://cdn.cloudflare.steamstatic.com/steam/apps/481510/ss_e586930c3199b402969aee59f1b74d46db42af0d.1920x1080.jpg?t=1706292417', N'https://store.steampowered.com/app/481510/Night_in_the_Woods/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (15, N'Slime Rancher 2', N'Continue the adventures of Beatrix LeBeau as she journeys across the Slime Sea to Rainbow Island, a land brimming with ancient mysteries, and bursting with wiggly, new slimes to wrangle in this sequel to the smash-hit, Slime Rancher.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1657630/header.jpg?t=1707415250', N'https://i.3djuegos.com/juegos/18044/slime_rancher_2/fotos/ficha/slime_rancher_2-5748590.webp', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1657630/ss_f55291cebddb799a3d505a97a89fb5dbe27f3f5e.1920x1080.jpg?t=1707415250', N'https://store.steampowered.com/app/1657630/Slime_Rancher_2/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (16, N'A Space for the Unbound', N'A beautifully pixelated adventure game set in rural Indonesia in the late 90s that tells the story of overcoming anxiety, depression, and the relationship between a boy and a girl with supernatural powers.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1201270/header.jpg?t=1704699294', N'https://static.wixstatic.com/media/4d4796_aa72d8a9f47645dfac04cb2537ee97e5~mv2.jpg/v1/fill/w_360,h_509,al_c,q_80,usm_0.66_1.00_0.01,enc_auto/4d4796_aa72d8a9f47645dfac04cb2537ee97e5~mv2.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1201270/ss_494bd4858dc7a71a33e32dde02af44b5f640c0cd.1920x1080.jpg?t=1704699294', N'https://store.steampowered.com/app/1201270/A_Space_for_the_Unbound/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (17, N'Animal Crossing: New Horizons', N'Animal Crossing: New Horizons is a Nintendo Switch game where players build a community on a deserted island with cute animal characters. They can fish, catch bugs, decorate their island, and visit other players islands for trading and socializing. Its a relaxing and charming game loved by players of all ages.', N'https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_656/b_white/f_auto/q_auto/ncom/software/switch/70010000027619/9989957eae3a6b545194c42fec2071675c34aadacd65e6b33fdfe7b3b6a86c3a', N'https://i.ebayimg.com/images/g/aNgAAOSwVChgEZxF/s-l500.jpg', N'https://www.hollywoodreporter.com/wp-content/uploads/2020/03/switch_acnh_nd0220_screen_03-h_2020.jpg?w=1296&h=730&crop=1', N'https://www.nintendo.es/Juegos/Juegos-de-Nintendo-Switch/Animal-Crossing-New-Horizons-1438623.html')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (18, N'Sky: Children of the Light', N'Sky: Children of the Light is a peaceful, award-winning MMO that is designed to help players connect with each other. You are a child of light with a magical cloak in search of others like you. Everyone is welcome in this cozy open-world adventure!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2325290/header.jpg?t=1709168578', N'https://cdn.hobbyconsolas.com/sites/navi.axelspringer.es/public/media/image/2019/07/sky-juego-ficha.jpg?tf=256x', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2325290/ss_b49f6872f3efa4b798c3ce50d7c9b2d4b4fdc460.1920x1080.jpg?t=1709168578', N'https://store.steampowered.com/app/2325290/Sky_Nios_de_la_Luz/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (19, N'Garden Buddies', N'Unleash your inner gardener and immerse yourself in the serene world of Garden Friends, growing a vibrant flower garden alongside your new friend Mutsy. Discover a charming and satisfying experience to release your stress.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2429090/header.jpg?t=1697796915', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmTWqCYwW-DRdMJyhrT5lUQKFqqbwJOPBdfvQaFan2aI-uRALa', N'https://cdn.cloudflare.steamstatic.com/steam/apps/2429090/ss_c9421ffc516f700073d1e579f5b748a8bada4ee6.1920x1080.jpg?t=1697796915', N'https://store.steampowered.com/app/2429090/Garden_Buddies/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (20, N'With You', N'With You is a cooperative two-player puzzle-playground. Its a tiny game that hopes to inspire couples to connect, communicate, and collaborate.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1860570/header.jpg?t=1655243989', N'https://howlongtobeat.com/GAMES/103909_With_You.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1860570/ss_fb3d9fcbaaa34ad8f7fb45c9348ade0dad717854.1920x1080.jpg?t=1655243989', N'https://store.steampowered.com/app/1860570/With_You/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (21, N'Bread & Fred', N'Join Bread and Fred! A rage game where you will have to cooperate to jump and climb to the mountain peak. Of course, you will have to be very careful not to fall, and not drag your penguin companion down the road... Can you overcome the route to the top with a friend?', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1607680/header.jpg?t=1707226764', N'https://howlongtobeat.com/GAMES/126715_Bread_&_Fred.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1607680/ss_7e1f28d600cb16d3ec858b3a76678bc9adbc1703.1920x1080.jpg?t=1707226764', N'https://store.steampowered.com/app/1607680/Bread__Fred/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (22, N'Untitled Goose Game', N'Its a beautiful day in town and you are a very bad goose.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/837470/header.jpg?t=1699052664', N'https://upload.wikimedia.org/wikipedia/en/b/b2/Untitled_Goose_Game_Cover_art.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/837470/ss_7576323f465966792d0fb021acf5a8a81a16e9f9.1920x1080.jpg?t=1699052664', N'https://store.steampowered.com/app/837470/Untitled_Goose_Game/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (23, N'Pikuniku', N'Pikuniku is an absurdly wonderful exploration game that takes place in a strange but playful world where not everything is as happy as it seems. Help strange characters overcome difficulties and start a small revolution in this delicious dystopian adventure!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/572890/header.jpg?t=1708463697', N'https://images.pcgamingwiki.com/0/0b/Pikuniku_cover.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/572890/ss_debaf976a994b833851ec930c52a369322f781ca.1920x1080.jpg?t=1708463697', N'https://store.steampowered.com/app/572890/Pikuniku/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (24, N'Forager', N'The popular and quirky "idle game that you dont want to stop playing." Explore, craft, gather and manage resources, find treasures and secrets and build your base from scratch! Buy land to explore and expand!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/751780/header.jpg?t=1699372258', N'https://upload.wikimedia.org/wikipedia/en/c/c2/Forager_game_cover.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/751780/ss_288c1647ffd3639358505e4e0e5a74214a9a9a23.1920x1080.jpg?t=1699372258', N'https://store.steampowered.com/app/751780/Forager/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (25, N'Florence', N'At 25 years old, Florence Yeoh feels like she is at a dead end. Her life is a constant routine of work, hours of sleep and excessive periods on social media. But one day she meets a cellist named Krish who completely turns her perspective on the world upside down.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1102130/header.jpg?t=1709314818', N'https://howlongtobeat.com/GAMES/53304_Florence.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1102130/ss_fe7d6ef1f2bf34692e29f8650c432df1908f914b.1920x1080.jpg?t=1709314818', N'https://store.steampowered.com/app/1102130/Florence/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (26, N'Kind Words', N'A game about writing nice letters to real people. Write and receive encouraging letters in a cozy room. Trade stickers and listen to chill music. We are all in this together. Sometimes all you need are a few kind words.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1070710/header.jpg?t=1701888082', N'https://m.media-amazon.com/images/M/MV5BOGQ1OTJkMDUtNGNmMi00YTNhLThmYTktZDcyMmZlMDQzNDY4XkEyXkFqcGdeQXVyNzU3Nzk4MDQ@._V1_.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1070710/ss_a52c837288c5bc1952430029f0edb9cf59f633b7.1920x1080.jpg?t=1701888082', N'https://store.steampowered.com/app/1070710/Kind_Words_lo_fi_chill_beats_to_write_to/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (27, N'Wobbledogs', N'Raise your own pack of mutant dogs with this 3D pet simulator. You can see even their intestines! Wobbledogs is a calm sandbox game for all those who want to take care of their own virtual pets in a unique, surprising and stress-free environment.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1424330/header.jpg?t=1702388533', N'https://howlongtobeat.com/GAMES/89154_Wobbledogs.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1424330/ss_f04e65a7a0708ef03b5de7c6fe362bc2dd2c75b1.1920x1080.jpg?t=1702388533', N'https://store.steampowered.com/app/1424330/Wobbledogs/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (28, N'Loddlenaut', N'Play as an interstellar guardian on a mission to clean up a polluted ocean planet. He picks up trash, explores animated waters, and cares for alien axolotl-like creatures!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1644940/header.jpg?t=1706732992', N'https://howlongtobeat.com/GAMES/94216_Loddlenaut.jpg?width=250', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1644940/ss_2d1795c076f9dfa96f99323c46be3aaac3babdfd.1920x1080.jpg?t=1706732992', N'https://store.steampowered.com/app/1644940/Loddlenaut/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (29, N'ASTRONEER', N'Explore and shape distant worlds! Astroneer is set during the gold rush of the 25th century. Players must explore the frontiers of outer space, risking their lives and resources in hostile environments in an attempt to achieve desired wealth.', N'https://cdn.cloudflare.steamstatic.com/steam/apps/361420/header.jpg?t=1701658983', N'https://cdn-products.eneba.com/resized-products/-LMWwlLgSQjMJNs079n9XoCRM2sU62rBoGWZ0b_Oq44_350x200_1x-0.jpeg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/361420/ss_858b8bece04b753a6b35a009776a4de9dd6e0df7.1920x1080.jpg?t=1701658983', N'https://store.steampowered.com/app/361420/ASTRONEER/')
INSERT [dbo].[JUEGO] ([IDJUEGO], [TITULO], [DESCRIPCION], [IMAGENBANNER], [IMAGENV], [IMAGENH], [ENLACE]) VALUES (30, N'The Sims™ 4', N'Enjoy the power to create and control people in a virtual world where there are no rules. Exercise your power with total freedom, have fun and play with life!', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1222670/header.jpg?t=1710328511', N'https://m.media-amazon.com/images/I/71otyq1xFNL.__AC_SX300_SY300_QL70_ML2_.jpg', N'https://cdn.cloudflare.steamstatic.com/steam/apps/1222670/ss_537683e5e29c2d6d02c64aa7321dcb26166f7d82.1920x1080.jpg?t=1710328511', N'https://store.steampowered.com/app/1222670/The_Sims_4/')
GO
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (6, 20, 1)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (7, 20, 2)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (8, 21, 2)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (9, 22, 3)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (10, 21, 3)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (11, 20, 3)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (12, 22, 1)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (13, 23, 1)
INSERT [dbo].[LIKE_COMENTARIO] ([IDLIKE], [IDRATING], [IDUSUARIO]) VALUES (14, 21, 1)
GO
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (3, 11, 1, CAST(9.00 AS Decimal(5, 2)), N'This game is awesome', N'Really beautiful game with colorful scenarios', CAST(N'2024-03-18T19:11:59.757' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (7, 13, 1, CAST(8.00 AS Decimal(5, 2)), N'Penguins dont fly', N'I really like this game, but a little bit short', CAST(N'2024-03-18T19:16:16.223' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (10, 1, 3, CAST(8.00 AS Decimal(5, 2)), N'Un encantador escaparate empresarial', N'Sticky Business ofrece una experiencia acogedora y adictiva al dirigir tu propio negocio de stickers. Sumérgete en la gestión mientras construyes tu tienda en línea y escuchas las historias de tus clientes. ¡Una delicia empaquetada en una aventura encantadora!', CAST(N'2024-03-18T23:17:01.180' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (11, 2, 3, CAST(9.00 AS Decimal(5, 2)), N'Una vida rural repleta de encanto', N'Stardew Valley te invita a escapar a la vida agrícola, donde puedes cultivar tus propios cultivos, criar animales y conocer a una variedad de personajes entrañables. Una experiencia cautivadora que te sumerge en un mundo lleno de posibilidades.', CAST(N'2024-03-18T23:17:18.930' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (12, 3, 3, CAST(7.00 AS Decimal(5, 2)), N'Una charla relajante con un toque de fantasía', N'Coffee Talk ofrece una experiencia tranquila y conmovedora al escuchar los problemas de los clientes mientras preparas bebidas reconfortantes. Aunque la jugabilidad puede volverse repetitiva, el ambiente acogedor y las historias interesantes mantienen el interés.', CAST(N'2024-03-18T23:17:36.443' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (13, 4, 3, CAST(8.00 AS Decimal(5, 2)), N'Rebeldía en forma de rábano', N' En Turnip Boy Commits Tax Evasion, te embarcas en una aventura absurda y divertida como un rabanito travieso. Con su humor ingenioso y jugabilidad única, este juego ofrece una experiencia refrescante y entretenida que seguramente te sacará una sonrisa.', CAST(N'2024-03-18T23:18:04.287' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (14, 5, 3, CAST(9.00 AS Decimal(5, 2)), N'Un refugio encantado para el alma', N'Cozy Grove te transporta a una isla misteriosa donde puedes ayudar a los fantasmas locales y descubrir secretos ocultos. Con su estética encantadora y mecánicas relajantes, este juego te invita a explorar y crear tu propio rincón acogedor.', CAST(N'2024-03-18T23:18:26.350' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (15, 7, 3, CAST(8.00 AS Decimal(5, 2)), N'Un viaje artístico en la nieve', N'Blanc es una hermosa aventura cooperativa que sigue el viaje de una cría de lobo y un cervatillo en un vasto y nevado páramo. Con su arte impresionante y su historia conmovedora, este juego ofrece una experiencia emocionalmente resonante que te invita a explorar y conectar con los personajes.', CAST(N'2024-03-18T23:18:48.000' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (16, 9, 3, CAST(7.00 AS Decimal(5, 2)), N'Ordenando el caos con estilo', N'A Little to the Left es un juego de rompecabezas acogedor que desafía tu habilidad para organizar objetos de manera ordenada. Aunque su premisa es simple y su diseño visual encantador, la falta de variedad en los desafíos puede hacer que la experiencia se sienta un poco monótona.', CAST(N'2024-03-18T23:19:05.570' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (17, 11, 3, CAST(9.00 AS Decimal(5, 2)), N'Un viaje emocional a través de la belleza visual', N'Gris es una obra maestra visual y narrativa que explora temas emocionales profundos. Con su estética impresionante y su narrativa conmovedora, este juego ofrece una experiencia emocionalmente impactante que deja una impresión duradera.', CAST(N'2024-03-18T23:19:27.590' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (18, 12, 1, CAST(8.00 AS Decimal(5, 2)), N'El paraíso de los amantes de los gatos', N'Calico te sumerge en un mundo lleno de encanto y ternura, donde puedes construir y gestionar tu propia cafetería de gatos. Con su estética adorable y su enfoque relajante, este juego ofrece una experiencia reconfortante que te dejará sonriendo entre felinos.', CAST(N'2024-03-18T23:21:07.127' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (20, 3, 1, CAST(5.00 AS Decimal(5, 2)), N'Cafe charla', N'Me ha gustado pero un poco lento no se', CAST(N'2024-03-18T23:33:07.767' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (21, 30, 2, CAST(7.00 AS Decimal(5, 2)), N'Es dios', N'Me encanta tio son los sims ', CAST(N'2024-03-18T23:34:53.430' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (22, 22, 3, CAST(2.00 AS Decimal(5, 2)), N'Overrated', N'no se sin mas', CAST(N'2024-03-18T23:35:32.453' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (23, 19, 1, CAST(7.00 AS Decimal(5, 2)), N'Cutie', N'Suuuper cute ??????', CAST(N'2024-03-18T23:44:00.593' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (24, 11, 1, CAST(10.00 AS Decimal(5, 2)), N'super', N'un 10 de lejos', CAST(N'2024-03-18T23:56:30.083' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (25, 4, 1, CAST(6.00 AS Decimal(5, 2)), N'Boy', N'Oh boy oh boy I love turnips', CAST(N'2024-03-19T00:07:28.613' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (26, 18, 1, CAST(2.00 AS Decimal(5, 2)), N'Meh', N'No me gusto nada', CAST(N'2024-03-19T00:07:55.053' AS DateTime))
INSERT [dbo].[RATING] ([IDRATING], [IDJUEGO], [IDUSUARIO], [NOTA], [TITULO], [COMENTARIO], [FECHA]) VALUES (27, 12, 1, CAST(5.00 AS Decimal(5, 2)), N'Cutie', N'Cute cats', CAST(N'2024-03-19T00:08:15.767' AS DateTime))
GO
INSERT [dbo].[USUARIO] ([IDUSUARIO], [NOMBRE], [EMAIL], [FOTO], [PASS], [PASSORIGEN], [SALT]) VALUES (1, N'Alexia', N'alexia@mail.com', N'img1.jpeg', 0x7F3237673D19F9FD626C480CD20227F678807362CE956F7315A3CA9F98E828B2DCD5BDABBEE56B5F434F3F8A3EBC2F2C8EEBE2B1B7B4EB634379C1F83452D298, N'12345', N'¦½_\edß¢ÌM?¦ÂìQ+¯JxkàPäPú¿©UÚÛ;PøÍ/eH')
INSERT [dbo].[USUARIO] ([IDUSUARIO], [NOMBRE], [EMAIL], [FOTO], [PASS], [PASSORIGEN], [SALT]) VALUES (2, N'zan', N'zan@gmail.com', N'img2.jpeg', 0xB8B542E6E0839C0797C9111164C6B4235FF93BE564F2828041CEFDA487F53082EABBB9C388AA75E90508A5B4B4721843A12163C3EEF1DBBB4C3F69CFB1D81ED3, N'12345', N'ÍñJêC|! ]9ÑB^zLúSôVÎ^1]¸%Ë§èûÏ@+­±»åà5x')
INSERT [dbo].[USUARIO] ([IDUSUARIO], [NOMBRE], [EMAIL], [FOTO], [PASS], [PASSORIGEN], [SALT]) VALUES (3, N'ProcedimientoSQL', N'prueba@mail.com', N'img3.jpeg',0x91A62730AD5354B31B76BE52424489DD9FBE88034F4807CD325734852472F91F56B7EDDE1DC16E3101F56C91B2C540CD44735069ED264C3363498BD3AA14D672 , N'12345', N'ÞË©>B1¶ÃÂn"þ?Âbaû·Îªá~AÉ2p$àyµÅ SåE¢ã')
GO
ALTER TABLE [dbo].[LIKE_COMENTARIO]  WITH CHECK ADD FOREIGN KEY([IDRATING])
REFERENCES [dbo].[RATING] ([IDRATING])
GO
ALTER TABLE [dbo].[LIKE_COMENTARIO]  WITH CHECK ADD FOREIGN KEY([IDUSUARIO])
REFERENCES [dbo].[USUARIO] ([IDUSUARIO])
GO
ALTER TABLE [dbo].[RATING]  WITH CHECK ADD FOREIGN KEY([IDJUEGO])
REFERENCES [dbo].[JUEGO] ([IDJUEGO])
GO
ALTER TABLE [dbo].[RATING]  WITH CHECK ADD FOREIGN KEY([IDUSUARIO])
REFERENCES [dbo].[USUARIO] ([IDUSUARIO])
GO

