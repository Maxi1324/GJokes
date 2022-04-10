

export function SendPost(Route: string, body1: any, Callback: any, Authenticate: boolean = true) {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    if (Authenticate) {
        myHeaders.append("Authorization", "Bearer " + sessionStorage.getItem("JWTToken"));
    }

    var raw = JSON.stringify(body1);

    var requestOptions: any = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
    };

    fetch(sessionStorage.getItem("BackendRoute") + Route, requestOptions)
        .then(response => response.json())
        .then(Callback)
        .catch(error => console.log('error', error));
}

export interface UserCred {
    Email: string,
    Password: string
}

export interface ConfigInfos {
    minCon:number,
    maxCon:number,
    repoName:string,

    erstellung:Date,

    githubEmail:string,
    githubUsername:string
}

export interface ConRange{
    minCon:number,
    maxCon:number
}

export interface GithubAccountSettings{
    githubEmail:number,
    githubUserName:number
}

export interface RepoName{
    name:string
}

export interface Response{
    code:number,
    desc:string
}

export interface UserCred{
    email:string,
    password:string
}

export interface UserInfo{
    userId:number
}