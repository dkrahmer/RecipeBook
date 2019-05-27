# AppUserId, Username, FirstName, LastName, CanViewRecipe, CanEditRecipe, IsAdmin, LastLoggedInDate, CreateDateTime, UpdateDateTime

-- Add a new user that has never logged in before
INSERT INTO AppUsers (Username, FirstName, LastName, CanViewRecipe, CanEditRecipe, IsAdmin)
VALUES ('______@gmail.com', '<First>', '<Last>', 1, 1, 1);


-- Give permissions to an existing user
UPDATE AppUsers
SET
	CanViewRecipe = 1,
	CanEditRecipe = 1,
	IsAdmin = 1
WHERE Username = '______@gmail.com'
