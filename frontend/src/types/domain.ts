export interface DomainsDto {
    id:          number;
    name:        string;
    isActive:    boolean;
    createdAt:   Date;
    updatedAt:   null;
    recordCount: number;
}

export interface DomainAvailabilityDto {
    domainName:  string;
    isAvailable: boolean;
    message:     string;
    status:      string;
}

export interface CreateDomainDto {
    name:     string;
}

export interface UpdateDomainDto {
    name:     string;
    isActive: boolean;
}