CREATE TABLE `Recipes` (
  `RecipeId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` text,
  `Ingredients` text,
  `Instructions` text,
  `CreateDateTime` datetime NOT NULL,
  `UpdateDateTime` datetime NOT NULL,
  `Notes` text,
  `RecipeGroupId` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`RecipeId`),
  UNIQUE KEY `RecipeId_UNIQUE` (`RecipeId`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;


CREATE TABLE `AppUsers` (
  `AppUserId` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) DEFAULT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `DefaultRecipeGroupId` int(11) NOT NULL DEFAULT '1',
  `LastLoggedInDate` datetime DEFAULT NULL,
  `CreateDateTime` datetime NOT NULL,
  `UpdateDateTime` datetime NOT NULL,
  PRIMARY KEY (`AppUserId`),
  UNIQUE KEY `Username_UNIQUE` (`Username`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;


-- associate users with recipe groups and roles
CREATE TABLE `AppUserRecipeGroups` (
  `AppUserId` int(11) NOT NULL,
  `RecipeGroupId` int(11) NOT NULL,
  `CanViewRecipe` tinyint(4) NOT NULL DEFAULT '0',
  `CanEditRecipe` tinyint(4) NOT NULL DEFAULT '0',
  `IsAdmin` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`AppUserId`, `RecipeGroupId`),
  UNIQUE KEY `Username_UNIQUE` (`Username`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 DEFAULT COLLATE=utf8_unicode_ci;


-- Character set and collate info: https://dba.stackexchange.com/questions/96265/mysql-silently-replaces-utf-chars-with-literal-question-marks
