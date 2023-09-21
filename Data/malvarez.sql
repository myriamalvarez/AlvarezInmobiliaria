-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-09-2023 a las 01:56:57
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
-- Base de datos: `malvarez`
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
  `InquilinoId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `FechaInicio`, `FechaFin`, `Alquiler`, `InmuebleId`, `InquilinoId`) VALUES
(7, '2023-08-01', '2025-08-01', 75000, 4, 4),
(8, '2023-05-01', '2025-05-01', 100000, 1, 1),
(9, '2023-06-01', '2025-06-01', 86000, 3, 3),
(12, '2023-10-02', '2023-11-02', 96000, 1, 1);

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
  `Estado` tinyint(1) NOT NULL,
  `Precio` decimal(8,2) NOT NULL,
  `PropietarioId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Latitud`, `Longitud`, `Estado`, `Precio`, `PropietarioId`) VALUES
(1, 'Alsina 325 San Luis', 1, 1, 3, 999.99999, -999.99999, 1, 100000.00, 1),
(3, 'Lafinur 488 San Luis', 2, 3, 1, 11.66000, -78.82000, 1, 86000.00, 3),
(4, 'Lavalle 293 P 1° B San Luis', 1, 2, 2, 999.99999, -58.32000, 1, 75000.00, 9),
(5, 'Pringles 555 Local 2 Merlo', 1, 1, 3, 32.31000, -95.21000, 0, 66000.00, 7),
(7, 'Falucho 555 San Luis', 1, 1, 2, 34.65420, -999.99999, 1, 63000.00, 12),
(8, 'España 136 San Luis', 2, 5, 1, 32.30200, -65.12500, 1, 130000.00, 1),
(9, 'Lafinur 48 San Luis', 1, 2, 2, 999.99999, -78.82000, 0, 50000.00, 13);

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
(3, 'Ariel', 'Montoya', '13890753', '0266-4058661', 'arielmontoya@gmail.com'),
(4, 'Rodrigo ', 'Diaz', '38454663', '0266-4058098', 'rodrigodiaz@gmail.com'),
(5, 'Oscar ', 'Moran', '16685614', '0266-4051446', 'oscarmoran@gmail.com'),
(6, 'Marcela', 'Ramirez', '34456988', '0266-4076969', 'marcelaramirez@gmail.com'),
(7, 'Jorge', 'Heredia', '40239856', '0266-4560230', 'jorgeheredia@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Importe` decimal(10,0) NOT NULL,
  `ContratoId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id`, `NumeroPago`, `Fecha`, `Importe`, `ContratoId`) VALUES
(8, 1, '0003-05-01', 100000, 8),
(9, 2, '2023-06-01', 100000, 8),
(10, 1, '2023-09-01', 75000, 7),
(11, 3, '2023-07-01', 100000, 8),
(12, 4, '2023-08-02', 100000, 8);

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
(7, 'Anibal', 'Tobio', '28874231', '0266-4466354', 'anibaltobio@gmail.com'),
(9, 'Jose', 'Romano', '35455278', '0266-4356814', 'joseromano@gmail.com'),
(11, 'Adriana', 'Gomez', '25889242', '0266-4548800', 'adrianagomez@gmail.com'),
(12, 'Mirta', 'Vall', '36854240', '0266-4591248', 'mirtavall@gmail.com'),
(13, 'Adriana', 'Funes', '25889242', '0266-4548800', 'aa@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Apellido` varchar(30) NOT NULL,
  `Avatar` varchar(100) DEFAULT NULL,
  `Email` varchar(50) NOT NULL,
  `Clave` varchar(100) NOT NULL,
  `Rol` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id`, `Nombre`, `Apellido`, `Avatar`, `Email`, `Clave`, `Rol`) VALUES
(2, 'Myriam', 'Alvarez', '/avatar\\usuario_2.jpg', 'myriamalvarez@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 1),
(3, 'Luis', 'Alvarez', '/avatar\\usuario_5.jpg', 'luisalvarez@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 2),
(4, 'Luisa', 'Alvarez', '/avatar\\Nusuario_4.jpg', 'luisaalvarez@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 2),
(5, 'Juan', 'Perez', '/avatar\\usuario_5.png', 'juanperez@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 2),
(6, 'Carla', 'Merlo', '/avatar\\Nusuario_6.jpg', 'carlamerlo@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 2),
(7, 'Raul', 'Aguilar', '/avatar\\Nusuario_7.jpg', 'raul@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 1),
(9, 'Karin', 'Juarez', '/avatar\\Nusuario_9.jpg', 'karijuarez@gmail.com', 'Kl6IbrJVaAWPV8R9InNl7W7BgmYd7fb3Bco4f/g6y0c=', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `InquilinoId` (`InquilinoId`),
  ADD KEY `InmuebleId` (`InmuebleId`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `pago_ibfk_1` (`ContratoId`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilino` (`Id`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`Id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`Id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contrato` (`Id`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
