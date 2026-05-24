CREATE DATABASE RecetarioDB COLLATE Latin1_General_CI_AI;
GO

USE RecetarioDB;
GO

CREATE TABLE Clasificaciones (
    IdClasificacion INT          IDENTITY(1,1) NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    CONSTRAINT PK_Clasificaciones       PRIMARY KEY (IdClasificacion),
    CONSTRAINT UQ_Clasificaciones_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE Unidades (
    IdUnidad    INT         IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(50) NOT NULL,
    Abreviatura VARCHAR(10) NOT NULL,
    CONSTRAINT PK_Unidades             PRIMARY KEY (IdUnidad),
    CONSTRAINT UQ_Unidades_Nombre      UNIQUE (Nombre),
    CONSTRAINT UQ_Unidades_Abreviatura UNIQUE (Abreviatura)
);
GO

CREATE TABLE Usuarios (
    IdUsuario       INT          IDENTITY(1,1) NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    Apellido        VARCHAR(100) NOT NULL,
    Email           VARCHAR(150) NOT NULL,
    Password        VARCHAR(255) NOT NULL,
    Rol             VARCHAR(30)  NOT NULL,
    IdClasificacion INT          NULL,
    CONSTRAINT PK_Usuarios                 PRIMARY KEY (IdUsuario),
    CONSTRAINT FK_Usuarios_Clasificaciones FOREIGN KEY (IdClasificacion) REFERENCES Clasificaciones(IdClasificacion),
    CONSTRAINT UQ_Usuarios_Email           UNIQUE (Email),
    CONSTRAINT CK_Usuarios_Rol             CHECK (Rol IN ('lider', 'admin'))
);
GO

CREATE TABLE Proveedores (
    IdProveedor INT          IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(100) NOT NULL,
    Contacto    VARCHAR(100) NULL,
    Telefono    VARCHAR(20)  NULL,
    Email       VARCHAR(150) NULL,
    Direccion   VARCHAR(255) NULL,
    CONSTRAINT PK_Proveedores PRIMARY KEY (IdProveedor)
);
GO

CREATE TABLE Equipo (
    IdIntegrante    INT          IDENTITY(1,1) NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    Apellido        VARCHAR(100) NOT NULL,
    IdClasificacion INT          NOT NULL,
    CONSTRAINT PK_Equipo                 PRIMARY KEY (IdIntegrante),
    CONSTRAINT FK_Equipo_Clasificaciones FOREIGN KEY (IdClasificacion) REFERENCES Clasificaciones(IdClasificacion)
);
GO

CREATE TABLE Ingredientes (
    IdIngrediente INT           IDENTITY(1,1) NOT NULL,
    Codigo        VARCHAR(20)   NOT NULL,
    Descripcion   VARCHAR(100)  NOT NULL,
    IdUnidad      INT           NOT NULL,
    StockActual   DECIMAL(10,4) NOT NULL DEFAULT 0,
    StockMinimo   DECIMAL(10,4) NOT NULL DEFAULT 0,
    CONSTRAINT PK_Ingredientes             PRIMARY KEY (IdIngrediente),
    CONSTRAINT FK_Ingredientes_Unidades    FOREIGN KEY (IdUnidad) REFERENCES Unidades(IdUnidad),
    CONSTRAINT UQ_Ingredientes_Codigo      UNIQUE (Codigo),
    CONSTRAINT CK_Ingredientes_StockMinimo CHECK (StockMinimo >= 0)
);
GO

CREATE TABLE Recetas (
    IdReceta        INT          IDENTITY(1,1) NOT NULL,
    Codigo          VARCHAR(20)  NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    IdClasificacion INT          NOT NULL,
    PorcionesBase   INT          NOT NULL,
    Activo          BIT          NOT NULL DEFAULT 1,
    Imagen          VARCHAR(255) NULL,
    CONSTRAINT PK_Recetas                 PRIMARY KEY (IdReceta),
    CONSTRAINT FK_Recetas_Clasificaciones FOREIGN KEY (IdClasificacion) REFERENCES Clasificaciones(IdClasificacion),
    CONSTRAINT UQ_Recetas_Codigo          UNIQUE (Codigo),
    CONSTRAINT CK_Recetas_PorcionesBase   CHECK (PorcionesBase > 0)
);
GO

CREATE TABLE PrecioxIngrediente (
    IdIngrediente INT           NOT NULL,
    IdProveedor   INT           NOT NULL,
    Precio        DECIMAL(12,4) NOT NULL,
    FechaVigencia DATE          NOT NULL,
    CONSTRAINT PK_PrecioxIngrediente              PRIMARY KEY (IdIngrediente, IdProveedor),
    CONSTRAINT FK_PrecioxIngrediente_Ingredientes FOREIGN KEY (IdIngrediente) REFERENCES Ingredientes(IdIngrediente),
    CONSTRAINT FK_PrecioxIngrediente_Proveedores  FOREIGN KEY (IdProveedor)   REFERENCES Proveedores(IdProveedor),
    CONSTRAINT CK_PrecioxIngrediente_Precio       CHECK (Precio > 0)
);
GO

CREATE TABLE IngredientesxRecetas (
    IdReceta      INT           NOT NULL,
    IdIngrediente INT           NOT NULL,
    CantNeta      DECIMAL(10,4) NOT NULL,
    Rendimiento   DECIMAL(5,2)  NOT NULL DEFAULT 100,
    CantBruta     DECIMAL(10,4) NOT NULL,
    IdUnidad      INT           NOT NULL,
    CONSTRAINT PK_IngredientesxRecetas             PRIMARY KEY (IdReceta, IdIngrediente),
    CONSTRAINT FK_IngredientesxRecetas_Recetas      FOREIGN KEY (IdReceta)      REFERENCES Recetas(IdReceta),
    CONSTRAINT FK_IngredientesxRecetas_Ingredientes FOREIGN KEY (IdIngrediente) REFERENCES Ingredientes(IdIngrediente),
    CONSTRAINT FK_IngredientesxRecetas_Unidades     FOREIGN KEY (IdUnidad)      REFERENCES Unidades(IdUnidad),
    CONSTRAINT CK_IngredientesxRecetas_Rendimiento  CHECK (Rendimiento > 0 AND Rendimiento <= 100),
    CONSTRAINT CK_IngredientesxRecetas_CantNeta     CHECK (CantNeta > 0)
);
GO

CREATE TABLE Procedimientos (
    IdProcedimiento INT          IDENTITY(1,1) NOT NULL,
    IdReceta        INT          NOT NULL,
    NroPaso         INT          NOT NULL,
    Descripcion     VARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Procedimientos         PRIMARY KEY (IdProcedimiento),
    CONSTRAINT FK_Procedimientos_Recetas FOREIGN KEY (IdReceta) REFERENCES Recetas(IdReceta),
    CONSTRAINT CK_Procedimientos_NroPaso CHECK (NroPaso > 0)
);
GO

CREATE TABLE Comandas (
    IdComanda    INT  IDENTITY(1,1) NOT NULL,
    IdReceta     INT  NOT NULL,
    Fecha        DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    Porciones    INT  NOT NULL,
    IdUsuario    INT  NOT NULL,
    IdIntegrante INT  NOT NULL,
    CONSTRAINT PK_Comandas          PRIMARY KEY (IdComanda),
    CONSTRAINT FK_Comandas_Recetas  FOREIGN KEY (IdReceta)     REFERENCES Recetas(IdReceta),
    CONSTRAINT FK_Comandas_Usuarios FOREIGN KEY (IdUsuario)    REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Comandas_Equipo   FOREIGN KEY (IdIntegrante) REFERENCES Equipo(IdIntegrante),
    CONSTRAINT CK_Comandas_Porciones CHECK (Porciones > 0)
);
GO

CREATE TABLE Modificaciones (
    IdModificacion         INT           IDENTITY(1,1) NOT NULL,
    IdComanda              INT           NOT NULL,
    Tipo                   VARCHAR(20)   NOT NULL,
    IdIngredienteOriginal  INT           NULL,
    IdIngredienteReemplazo INT           NULL,
    Cantidad               DECIMAL(10,4) NOT NULL,
    IdUnidad               INT           NOT NULL,
    CONSTRAINT PK_Modificaciones                       PRIMARY KEY (IdModificacion),
    CONSTRAINT FK_Modificaciones_Comandas              FOREIGN KEY (IdComanda)              REFERENCES Comandas(IdComanda),
    CONSTRAINT FK_Modificaciones_IngredienteOriginal   FOREIGN KEY (IdIngredienteOriginal)  REFERENCES Ingredientes(IdIngrediente),
    CONSTRAINT FK_Modificaciones_IngredienteReemplazo  FOREIGN KEY (IdIngredienteReemplazo) REFERENCES Ingredientes(IdIngrediente),
    CONSTRAINT FK_Modificaciones_Unidades              FOREIGN KEY (IdUnidad)               REFERENCES Unidades(IdUnidad),
    CONSTRAINT CK_Modificaciones_Tipo                  CHECK (Tipo IN ('sustitucion', 'adicion', 'eliminacion')),
    CONSTRAINT CK_Modificaciones_Cantidad              CHECK (Cantidad > 0)
);
GO

CREATE TABLE Costos (
    IdCosto       INT           IDENTITY(1,1) NOT NULL,
    IdReceta      INT           NOT NULL,
    Fecha         DATE          NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    Porciones     INT           NOT NULL,
    CostoTotal    DECIMAL(12,4) NOT NULL,
    CostoUnitario DECIMAL(12,4) NOT NULL,
    IdUsuario     INT           NOT NULL,
    CONSTRAINT PK_Costos          PRIMARY KEY (IdCosto),
    CONSTRAINT FK_Costos_Recetas  FOREIGN KEY (IdReceta)  REFERENCES Recetas(IdReceta),
    CONSTRAINT FK_Costos_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT CK_Costos_Porciones CHECK (Porciones > 0)
);
GO

CREATE TABLE MovimientosStock (
    IdMovimiento  INT           IDENTITY(1,1) NOT NULL,
    IdIngrediente INT           NOT NULL,
    Tipo          VARCHAR(20)   NOT NULL,
    Cantidad      DECIMAL(10,4) NOT NULL,
    IdUnidad      INT           NOT NULL,
    Fecha         DATETIME      NOT NULL DEFAULT GETDATE(),
    IdUsuario     INT           NOT NULL,
    Observaciones VARCHAR(255)  NULL,
    CONSTRAINT PK_MovimientosStock              PRIMARY KEY (IdMovimiento),
    CONSTRAINT FK_MovimientosStock_Ingredientes FOREIGN KEY (IdIngrediente) REFERENCES Ingredientes(IdIngrediente),
    CONSTRAINT FK_MovimientosStock_Unidades     FOREIGN KEY (IdUnidad)      REFERENCES Unidades(IdUnidad),
    CONSTRAINT FK_MovimientosStock_Usuarios     FOREIGN KEY (IdUsuario)     REFERENCES Usuarios(IdUsuario),
    CONSTRAINT CK_MovimientosStock_Tipo         CHECK (Tipo IN ('entrada', 'salida', 'ajuste')),
    CONSTRAINT CK_MovimientosStock_Cantidad     CHECK (Cantidad > 0)
);
GO
