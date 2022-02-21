-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: com2us
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `playerrobotmon`
--

DROP TABLE IF EXISTS `playerrobotmon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerrobotmon` (
  `PlayerID` varchar(20) NOT NULL,
  `RobotmonID` int NOT NULL,
  `Level` int NOT NULL,
  `HP` int NOT NULL,
  `Star` int NOT NULL,
  `CatchedDate` datetime NOT NULL,
  `CatchedLocX` int NOT NULL,
  `CatchedLocY` int NOT NULL,
  `Reinforcement` int NOT NULL,
  KEY `playerrobotmon_ibfk_1` (`RobotmonID`),
  KEY `fk_playerrobotmonID` (`PlayerID`),
  CONSTRAINT `fk_playerrobotmonID` FOREIGN KEY (`PlayerID`) REFERENCES `gameplayer` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `playerrobotmon_ibfk_1` FOREIGN KEY (`RobotmonID`) REFERENCES `robotmon` (`RobotmonID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `playerrobotmon`
--

LOCK TABLES `playerrobotmon` WRITE;
/*!40000 ALTER TABLE `playerrobotmon` DISABLE KEYS */;
/*!40000 ALTER TABLE `playerrobotmon` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-02-21 16:05:14
