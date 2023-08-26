-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-08-2023 a las 08:10:03
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `alvarez`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `Alquiler` decimal(10,0) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `Estado` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `FechaInicio`, `FechaFin`, `Alquiler`, `InmuebleId`, `InquilinoId`, `Estado`) VALUES
(1, '2023-09-01', '2024-09-01', 100000, 1, 1, 'Vigente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(100) NOT NULL,
  `Uso` int(4) NOT NULL,
  `Tipo` int(11) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Latitud` decimal(8,5) NOT NULL,
  `Longitud` decimal(8,5) NOT NULL,
  `Estado` int(11) NOT NULL,
  `Precio` decimal(8,2) NOT NULL,
  `PropietarioId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Latitud`, `Longitud`, `Estado`, `Precio`, `PropietarioId`) VALUES
(1, 'Alsina 325 San Luis', 1, 1, 5, 999.99999, -999.99999, 1, 100000.00, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(1, 'Luis', 'Gomez', '25889242', '0266-4548800', 'luisgomez@gmail.com'),
(3, 'Ariel', 'Montoya', '13890753', '0266-4058661', 'arielmontoya@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(1, 'Adrian', 'Gonzalez', '18889236', '0266-4566892', 'adriangonzalez@gmail.com'),
(3, 'Carlos', 'Ramirez', '23699241', '0266-4565879', 'carlosramirez@gmail.com'),
(5, 'Raul', 'Paz', '34158406', '0266-4032914', 'raulpaz@gmail.com'),
(6, 'Mariano', 'Lopez', '30344451', '0266-4058605', 'marianolopez@gmail.com'),
(7, 'Anibal', 'Tobio', '28874235', '0266-4466354', 'anibaltobio@gmail.com'),
(8, 'Gabriela', 'Medina', '35885212', '0266-4526358', 'gabrielamedina@gmail.com'),
(9, 'Jose', 'Romano', '35455278', '0266-4356814', 'joseromano@gmail.com'),
(11, 'Adriana', 'Gomez', '25889242', '0266-4548800', 'adrianagomez@gmail.com');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `InmuebleId` (`InmuebleId`,`InquilinoId`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
