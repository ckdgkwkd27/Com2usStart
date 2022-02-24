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
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account` (
  `ID` varchar(20) NOT NULL,
  `Password` varchar(90) NOT NULL,
  `Salt` varchar(65) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES ('test','01875fe21d94c2ac33bf417a9191dd1590e793b15618c6e4900d297f633db3f3','f6fx8dpelhqm4717gheew69eikyvshz3w6w17zac63qgvcc02dnolstljvxjygyk');
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gameplayer`
--

DROP TABLE IF EXISTS `gameplayer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gameplayer` (
  `UUID` varchar(45) NOT NULL,
  `ID` varchar(20) NOT NULL,
  `Level` int NOT NULL,
  `Exp` int NOT NULL,
  `GameMoney` int NOT NULL,
  `AttendDate` datetime NOT NULL,
  `GiftDate` datetime NOT NULL,
  `HowLongDays` int NOT NULL,
  PRIMARY KEY (`UUID`),
  KEY `fk_playerid` (`ID`),
  CONSTRAINT `fk_playerid` FOREIGN KEY (`ID`) REFERENCES `account` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gameplayer`
--

LOCK TABLES `gameplayer` WRITE;
/*!40000 ALTER TABLE `gameplayer` DISABLE KEYS */;
INSERT INTO `gameplayer` VALUES ('e263fc99-e99a-4ea3-9f9a-cfb9759825e0','test',1,0,0,'2022-02-24 14:56:58','2022-02-24 14:56:58',1);
/*!40000 ALTER TABLE `gameplayer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `UUID` varchar(45) NOT NULL,
  `ItemID` varchar(45) NOT NULL,
  `Amount` int NOT NULL DEFAULT '1',
  KEY `fk_invenUUID` (`UUID`),
  KEY `fk_invenItemID` (`ItemID`),
  CONSTRAINT `fk_invenItemID` FOREIGN KEY (`ItemID`) REFERENCES `item` (`ItemID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_invenUUID` FOREIGN KEY (`UUID`) REFERENCES `gameplayer` (`UUID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` VALUES ('e263fc99-e99a-4ea3-9f9a-cfb9759825e0','id_monsterball',100);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item` (
  `ItemID` varchar(45) NOT NULL,
  `ItemName` varchar(45) NOT NULL,
  `ItemType` varchar(45) NOT NULL,
  PRIMARY KEY (`ItemID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES ('id_gotchacoupon','가챠 쿠폰','TYPE_CONSUMABLE'),('id_money','돈','TYPE_MONEY'),('id_monsterball','몬스터볼','TYPE_MONSTERBALL'),('id_rarecostume','희귀 코스튬','TYPE_COSTUME'),('id_rescue','구급약','TYPE_CONSUMABLE'),('id_revive','부활석','TYPE_GOODS'),('id_treasuremap','보물지도','TYPE_MAP');
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mail`
--

DROP TABLE IF EXISTS `mail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mail` (
  `MailID` varchar(45) NOT NULL,
  `ItemID` varchar(45) DEFAULT NULL,
  `RecvID` varchar(20) DEFAULT NULL,
  `SendName` varchar(45) NOT NULL,
  `RecvDate` datetime NOT NULL,
  `Amount` int DEFAULT '0',
  `Title` varchar(90) NOT NULL,
  `Content` varchar(300) NOT NULL,
  `UUID` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`MailID`),
  KEY `fk_itemid` (`ItemID`),
  KEY `fk_mailUUID` (`UUID`),
  KEY `fk_recvid` (`RecvID`),
  CONSTRAINT `fk_itemid` FOREIGN KEY (`ItemID`) REFERENCES `item` (`ItemID`) ON DELETE CASCADE,
  CONSTRAINT `fk_mailUUID` FOREIGN KEY (`UUID`) REFERENCES `gameplayer` (`UUID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_recvid` FOREIGN KEY (`RecvID`) REFERENCES `gameplayer` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mail`
--

LOCK TABLES `mail` WRITE;
/*!40000 ALTER TABLE `mail` DISABLE KEYS */;
/*!40000 ALTER TABLE `mail` ENABLE KEYS */;
UNLOCK TABLES;

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
  `UUID` varchar(45) DEFAULT NULL,
  KEY `playerrobotmon_ibfk_1` (`RobotmonID`),
  KEY `fk_playerrobotmonID` (`PlayerID`),
  KEY `fk_playeruuid` (`UUID`),
  CONSTRAINT `fk_playeruuid` FOREIGN KEY (`UUID`) REFERENCES `gameplayer` (`UUID`) ON DELETE CASCADE ON UPDATE CASCADE,
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

--
-- Table structure for table `robotmon`
--

DROP TABLE IF EXISTS `robotmon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotmon` (
  `RobotmonID` int NOT NULL,
  `Name` varchar(45) NOT NULL,
  `Characteristic` varchar(45) NOT NULL,
  `Level` int NOT NULL,
  `HP` int NOT NULL,
  `Attack` int NOT NULL,
  `Defense` int NOT NULL,
  `Star` int NOT NULL,
  PRIMARY KEY (`RobotmonID`),
  UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `robotmon`
--

LOCK TABLES `robotmon` WRITE;
/*!40000 ALTER TABLE `robotmon` DISABLE KEYS */;
/*!40000 ALTER TABLE `robotmon` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-02-24 15:37:06
