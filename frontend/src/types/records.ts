export interface RecordsDto {
    id:         number;
    recordName: string;
    target:     string;
    priority:   number;
    ttl:        number;
    recordType: string;
}

export interface PutRecordsDto {
    recordName: string;
    target:     string;
    priority:   number;
    ttl:        number;
    recordType: string;
}

export interface CreateRecordsDto {
    domainId:   number;
    recordName: string;
    target:     string;
    priority:   number;
    ttl:        number;
    recordType: string;
}