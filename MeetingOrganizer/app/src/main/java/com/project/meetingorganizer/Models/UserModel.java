package com.project.meetingorganizer.Models;

public class UserModel
{
    private String userName;

    private String role;

    private int userId;

    public String getUserName(){
        return this.userName;
    }

    public void setUserName(String  userName)
    {
        this.userName = userName;
    }

    public void setUserId(int userId)
    {
        this.userId = userId;
    }

    public int getUserId(){
        return this.userId;
    }

    public String getRole(){
        return this.role;
    }

    public void setRole(String role){
        this.role = role;
    }
}
