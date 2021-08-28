/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 80021
 Source Host           : localhost:3306
 Source Schema         : inventory_management

 Target Server Type    : MySQL
 Target Server Version : 80021
 File Encoding         : 65001

 Date: 25/08/2021 10:39:45
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for emails
-- ----------------------------
DROP TABLE IF EXISTS `emails`;
CREATE TABLE `emails`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `Recipient` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Subject` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Body` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `FailedReson` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ResendAttempts` int(0) NULL DEFAULT NULL,
  `Status` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `CreatedDateTime` datetime(0) NULL DEFAULT NULL,
  `SentDateTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of emails
-- ----------------------------
INSERT INTO `emails` VALUES (1, 'yjayatilleke@gmail.com', 'sfsfsgdgd', 'fdgfhfhfghfghfghfh', NULL, 0, '0', '2021-08-25 00:09:02', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (2, 'InventoryManagement.BE.Merchant.Merchant,', 'Inventory Stock - 25/08/2021 12:12:18 AM', 'Code: , Name: , Qty: <br/>  sdsd ,  sfsf , 2000.00000 ', NULL, 0, '0', '2021-08-25 00:12:18', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (3, 'dwfsfrweteatetedwfwf', 'gfdgfd', 'jhgj', NULL, 0, '0', '2021-08-25 06:51:04', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (4, 'dwfsfrweteatetedwfwf', 'fyfkyf', 'hjf', NULL, 0, '0', '2021-08-25 06:52:24', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (5, 'dwfsfrweteatetedwfwf', 'vhg,cch', 'cmgcc', NULL, 0, '0', '2021-08-25 06:54:31', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (6, 'dwfsfrweteatetedwfwf,', 'Inventory Stock - 25/08/2021 7:50:05 AM', 'Code: , Name: , Qty: <br/>  sdsd ,  sfsf , 2,000.00  C002 ,  Produ 2 , 200.00 ', NULL, 0, '0', '2021-08-25 07:50:05', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (7, 'dwfsfrweteatetedwfwf,', 'Inventory Stock - 25/08/2021 8:34:10 AM', 'Code: , Name: , Qty: <br/>  sdsd ,  sfsf , 2,000.00  C002 ,  Produ 2 , 200.00 ', NULL, 0, '0', '2021-08-25 08:34:10', '0001-01-01 00:00:00');
INSERT INTO `emails` VALUES (8, 'yjayatilleke@gmial.com,', '<h1>Dear!, Inventory Stock at 25/08/2021 9:16:20 AM</p>', ' <h1> Code: , Name: , Qty:  </ p><br/>  sdsd ,  sfsf , 2,000.00  C002 ,  Produ 2 , 200.00 ', NULL, 0, '0', '2021-08-25 09:16:20', '0001-01-01 00:00:00');

-- ----------------------------
-- Table structure for merchants
-- ----------------------------
DROP TABLE IF EXISTS `merchants`;
CREATE TABLE `merchants`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `Code` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `EMail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of merchants
-- ----------------------------
INSERT INTO `merchants` VALUES (1, 'adass', 'sdsf', 'yjayatilleke@gmial.com');

-- ----------------------------
-- Table structure for products
-- ----------------------------
DROP TABLE IF EXISTS `products`;
CREATE TABLE `products`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `Code` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `CurrentQty` decimal(10, 5) NULL DEFAULT 0.00000,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of products
-- ----------------------------
INSERT INTO `products` VALUES (2, 'sdsd', 'sfsf', 2000.00000);
INSERT INTO `products` VALUES (3, 'C002', 'Produ 2', 200.00000);

-- ----------------------------
-- Table structure for security_privileges
-- ----------------------------
DROP TABLE IF EXISTS `security_privileges`;
CREATE TABLE `security_privileges`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `Code` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Value` tinyint(1) NULL DEFAULT 0,
  `UserId` int(0) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `FK_merchants_UserId`(`UserId`) USING BTREE,
  CONSTRAINT `FK_merchants_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of security_privileges
-- ----------------------------
INSERT INTO `security_privileges` VALUES (192, 'ProductAdd', 0, 2);
INSERT INTO `security_privileges` VALUES (193, 'EmailAdd', 0, 2);
INSERT INTO `security_privileges` VALUES (194, 'UserView', 0, 2);
INSERT INTO `security_privileges` VALUES (195, 'UserList', 0, 2);
INSERT INTO `security_privileges` VALUES (196, 'UserEdit', 0, 2);
INSERT INTO `security_privileges` VALUES (197, 'UserAdd', 0, 2);
INSERT INTO `security_privileges` VALUES (198, 'MerchantView', 0, 2);
INSERT INTO `security_privileges` VALUES (199, 'MerchantList', 0, 2);
INSERT INTO `security_privileges` VALUES (200, 'MerchantEdit', 0, 2);
INSERT INTO `security_privileges` VALUES (201, 'MerchantAdd', 0, 2);
INSERT INTO `security_privileges` VALUES (202, 'ProductView', 1, 2);
INSERT INTO `security_privileges` VALUES (203, 'ProductList', 1, 2);
INSERT INTO `security_privileges` VALUES (204, 'ProductEdit', 0, 2);
INSERT INTO `security_privileges` VALUES (205, 'EmailList', 0, 2);
INSERT INTO `security_privileges` VALUES (220, 'EmailBulkEmail', 1, 13);
INSERT INTO `security_privileges` VALUES (221, 'EmailAdd', 1, 13);
INSERT INTO `security_privileges` VALUES (222, 'UserView', 0, 13);
INSERT INTO `security_privileges` VALUES (223, 'UserList', 0, 13);
INSERT INTO `security_privileges` VALUES (224, 'UserEdit', 0, 13);
INSERT INTO `security_privileges` VALUES (225, 'UserAdd', 0, 13);
INSERT INTO `security_privileges` VALUES (226, 'MerchantView', 0, 13);
INSERT INTO `security_privileges` VALUES (227, 'MerchantList', 0, 13);
INSERT INTO `security_privileges` VALUES (228, 'MerchantEdit', 0, 13);
INSERT INTO `security_privileges` VALUES (229, 'MerchantAdd', 0, 13);
INSERT INTO `security_privileges` VALUES (230, 'ProductView', 1, 13);
INSERT INTO `security_privileges` VALUES (231, 'ProductList', 1, 13);
INSERT INTO `security_privileges` VALUES (232, 'ProductEdit', 1, 13);
INSERT INTO `security_privileges` VALUES (233, 'EmailList', 1, 13);
INSERT INTO `security_privileges` VALUES (234, 'ProductAdd', 1, 13);

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `FirstName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `LastName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Active` tinyint(1) NULL DEFAULT 0,
  `IsSuperUser` tinyint(1) NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'Admin', '82B537B20C1CF2EA0E384DB1F6BCE91D', 'Admin', 'Admin', 1, 1);
INSERT INTO `users` VALUES (2, 'Viwer', '82B537B20C1CF2EA0E384DB1F6BCE91D', 'Yuthika', 'Jayatillke', 1, 0);
INSERT INTO `users` VALUES (13, 'Kamal', '82B537B20C1CF2EA0E384DB1F6BCE91D', 'Manager', 'Namal', 1, 0);

SET FOREIGN_KEY_CHECKS = 1;
