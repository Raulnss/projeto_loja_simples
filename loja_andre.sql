CREATE DATABASE IF NOT EXISTS `loja_andre`;
USE `loja_andre`;

DROP TABLE IF EXISTS `produtos`;
CREATE TABLE IF NOT EXISTS `produtos` (
  `cd_id` int NOT NULL AUTO_INCREMENT,
  `nm_produto` varchar(140) NOT NULL,
  `vl_produto` varchar(140) NOT NULL,
  `qt_produto` varchar(140) NOT NULL,
  PRIMARY KEY (`cd_id`)
);

DROP TABLE IF EXISTS `tb_lista`;
CREATE TABLE IF NOT EXISTS `tb_lista` (
  `cd_id` int NOT NULL AUTO_INCREMENT,
  `dt_venda` varchar(30) NOT NULL,
  `hr_venda` varchar(30) NOT NULL,
  `ds_lista` varchar(10000) NOT NULL,
  PRIMARY KEY (`cd_id`)
);

DROP TABLE IF EXISTS `tb_update`;
CREATE TABLE IF NOT EXISTS `tb_update` (
  `cd_id` int NOT NULL AUTO_INCREMENT,
  `dt_produto` varchar(30) NOT NULL,
  `hr_venda` varchar(30) NOT NULL,
  `nm_produto` varchar(140) NOT NULL,
  `qt_produto` varchar(140) NOT NULL,
  PRIMARY KEY (`cd_id`)
);

DROP TABLE IF EXISTS `usuarios`;
CREATE TABLE IF NOT EXISTS `usuarios` (
  `cd_id` int NOT NULL AUTO_INCREMENT,
  `nm_user` varchar(20) NOT NULL,
  `sn_user` varchar(255) NOT NULL,
  PRIMARY KEY (`cd_id`)
);
COMMIT;
