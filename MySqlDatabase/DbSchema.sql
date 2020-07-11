CREATE TABLE `RecipesTest` (
  `RecipeId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` text,
  `Ingredients` text,
  `Instructions` text,
  `CreateDateTime` datetime NOT NULL,
  `UpdateDateTime` datetime NOT NULL,
  `Notes` text,
  PRIMARY KEY (`RecipeId`),
  UNIQUE KEY `RecipeId_UNIQUE` (`RecipeId`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;

CREATE TABLE `AppUsers` (
  `AppUserId` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) DEFAULT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `CanViewRecipe` tinyint(4) NOT NULL DEFAULT '0',
  `CanEditRecipe` tinyint(4) NOT NULL DEFAULT '0',
  `IsAdmin` tinyint(4) NOT NULL DEFAULT '0',
  `LastLoggedInDate` datetime DEFAULT NULL,
  `CreateDateTime` datetime NOT NULL,
  `UpdateDateTime` datetime NOT NULL,
  PRIMARY KEY (`AppUserId`),
  UNIQUE KEY `Username_UNIQUE` (`Username`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;


CREATE TABLE `DensityMapData` (
  `Name` varchar(100) NOT NULL,
  `AlternateNames` varchar(8000) DEFAULT NULL,
  `Density` decimal(16,13) NOT NULL,
  `Notes` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Name`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;

-- Character set and collate info: https://dba.stackexchange.com/questions/96265/mysql-silently-replaces-utf-chars-with-literal-question-marks