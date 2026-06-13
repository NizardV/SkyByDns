import { useEffect, useState } from "react"
import { toast } from "sonner"
import { useDebounce } from "@/lib/useDebounce"
import { domain } from "@/lib/api"
import DialogAdd from "./DialogAdd"

interface DialogAddContainerProps {
  refreshCallback: () => void
}

export default function DialogAddContainer({ refreshCallback }: DialogAddContainerProps) {
  const [name, setName] = useState("")
  const debouncedName = useDebounce(name, 500)

  const [isLoading, setIsLoading] = useState(false)
  const [domainExists, setDomainExists] = useState(false)
  const [error, setError] = useState("")
  const [canSubmit, setCanSubmit] = useState<boolean>(false)

  const handleChangeName = (event: React.ChangeEvent<HTMLInputElement>) => setName(event.target.value);

  // Feature : Declare a domain name
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    setIsLoading(true)

    try {
        await domain.create({ name })
        toast.success("Domain created successfully")
        setName("")
        refreshCallback()
    } catch (error) {
        toast.error("Error creating domain")
    } finally {
        setIsLoading(false)
    }
  }

  // Feature : Check domain availability
  useEffect(() => {
    if (debouncedName) {
        // check formatte of domain name
        const domainPattern = /^(?!-)(?:[a-zA-Z0-9-]{0,62}[a-zA-Z0-9]\.)+[a-zA-Z]{2,6}$/;
        if (!domainPattern.test(debouncedName)) {
            setDomainExists(null)
            setCanSubmit(false)
            setError("Invalid format for domain name")
            return;
        }

        // call API to check if domain exists
        const checkDomain = async () => {
            try {
                const isAvailable = await domain.checkAvailability(debouncedName)
                setDomainExists(!isAvailable)
                setCanSubmit(isAvailable)
                setError("")
            } catch (error) {
                setCanSubmit(false)
                toast.error("Error checking domain")
            }
        }
        checkDomain()
    }
  }, [debouncedName])

  return(<DialogAdd handleChangeName={handleChangeName} handleSubmit={handleSubmit} domainExists={domainExists} isLoading={isLoading} canSubmit={canSubmit} errorState={error} domainName={name}></DialogAdd>)
}

