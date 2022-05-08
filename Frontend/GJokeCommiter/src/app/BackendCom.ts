

export function SendPost(Route: string, body1: any, Callback: any, Authenticate: boolean = true) {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    if (Authenticate) {
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem("JWTToken"));
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

export function SendGet(Route: string, Callback: any, Authenticate: boolean = true, Parameter: any = {}, ErrorCallback: any = null) {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    if (Authenticate) {
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem("JWTToken"));
    }

    var requestOptions: any = {
        method: 'GET',
        headers: myHeaders,
    };

    let str = "";
    if (Object.keys(Parameter).length > 0) {
        str = "?";
        let isS = false;
        for (const key in Parameter) {
            if (isS) {
                str += "&";
            } else {
                isS = true;
            } 
            str += key + "=" + Parameter[key];
        }
    }

    fetch(sessionStorage.getItem("BackendRoute") + Route+str, requestOptions)
        .then(response => response.json())
        .then(Callback)
        .catch(ErrorCallback);
}

export function SendEmailVerification(UserId: number, Callback: any) {
    let U: UserInfo = {
        userId: UserId,
    }
    SendPost("api/Account/SendEmailVerification", U, Callback, false)
}

export interface UserCred {
    Email: string,
    Password: string
}

export interface ConfigInfos {
    minCon: number,
    maxCon: number,
    repoName: string,

    erstellung: Date,

    githubEmail: string,
    githubUsername: string
}

export interface ConRange {
    minCon: number,
    maxCon: number
}

export interface GithubAccountSettings {
    githubEmail: string,
    githubUserName: string
}

export interface RepoName {
    name: string
}

export interface Response {
    code: number,
    desc: string
}

export interface UserCred {
    email: string,
    password: string
}

export interface UserInfo {
    userId: number
}

export interface ActivateAccountInfo {
    userId: number,
    code: string
}

export interface ConfigInfos{
    minCon: number,
    maxCon: number,
    repoName: string,
    erstellung: Date,
    githubEmail: string,
    githubUsername: string
}

export interface HoleUserInfos{
    configInfos: ConfigInfos,
    realEmail: string,
    userId: number,
    blocked:boolean,
    verified:boolean
}

export enum Filter {
    All,
    Blocked,
    NotBlocked,
    NotAuthenticated,
    Authenticated
}

export enum OrderBy
{
    Email,
    Id,
    Joined,
    JoinedDesc
}

export interface GetAdminUserInfos{
    Page:number,
    OB:OrderBy,
    F:Filter,
    password:string
}

export interface BlockUser{
    password:string,
    userId:number
}