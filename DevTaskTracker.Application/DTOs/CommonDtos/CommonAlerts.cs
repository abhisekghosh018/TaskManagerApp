﻿namespace DevTaskTracker.Application.DTOs.Common
{
    public static class CommonAlerts
    {
        #region Registration Message
        public const string UserRegisteredSuccessfully = "User registered successfully.";
        public const string EmailAlreadyExists = "A user with this email already exists.";
        public const string UserAlreadyExists = "A user with this username already exists.";
        public const string OrganizationExists = "An organization with this name already exists.";
        public const string RoleAssignmentFailed = "Failed to assign default role.";
        #endregion

        #region Login Message  
        public const string LoginFailed = "Invalid Credentials.";
        public const string LoginSuccess = "Login successfull.";
        #endregion

        #region Member Message
        public const string MemberCreateSuccess = "A new member created successfull.";
        public const string MemberCreateFailed = "An error occoured while creating a member.";
        public const string MemberLoginFailed = "Invalid member Credentials.";
        public const string MemberLoginSuccess = "Member login successfull.";
        public const string NoMemberFound = "No member were found.";
        public const string MemberExistsWithEmail = "A member with this email already exists.";

        public const string MemberUpdated = "Member successfully updated.";
        public const string MemberUpdatedFaild = "Member update failed.";
        #endregion

        #region Task Message
        public const string TaskCreateSuccess = "A new task created successfull.";
        public const string TaskCreateFailed = "An error occoured while creating a task.";
        public const string TaskNotFound = "No Task were found based on query.";
        public const string TaskStatusUpdatedSuccessMessage = "Status updated successfully.";
        public const string TaskStatusUpdatedErrorMessage = "Status update failed.";
        #endregion
    }
}
