package com.project.meetingorganizer;

class ServerInfoProvider {
    private static final ServerInfoProvider ourInstance = new ServerInfoProvider();

    static ServerInfoProvider getInstance() {
        return ourInstance;
    }

    private String serverIP;

    private ServerInfoProvider() {
    }

    public void setServerIP(String serverIP) {
        this.serverIP = serverIP;
    }

    public String getVmsServiceApiUri()
    {
        return "http://" + serverIP + "/VMS/api/";
    }
}
