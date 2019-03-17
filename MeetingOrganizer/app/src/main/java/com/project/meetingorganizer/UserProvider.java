package com.project.meetingorganizer;

import com.project.meetingorganizer.Models.UserModel;

public class UserProvider {
    private static final UserProvider ourInstance = new UserProvider();

    public static UserProvider getInstance() {
        return ourInstance;
    }

    private UserProvider() {
    }

    private UserModel user;

    public void setUser(UserModel user){
        this.user = user;
    }

    public UserModel getUser(){
        return this.user;
    }
}
