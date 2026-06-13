import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { PlusCircle } from "lucide-react"
import { useState } from "react"
import { useNavigate } from "react-router-dom"

import Spinner from "@/components/ui/spinner"
import { record } from "@/lib/api"
import type { CreateRecordsDto } from "@/types/records"
import { toast } from "sonner"

export default function DialogAddRecords(domainIdProp?: { domainId: number }) {
  const navigate = useNavigate()
  const [isOpen, setIsOpen] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const [recordName, setRecordName] = useState("")
  const [target, setTarget] = useState("")
  const [priority, setPriority] = useState(0)
  const [ttl, setTtl] = useState(0)
  const [type, setType] = useState("")

  const handleChangeName = (event: React.ChangeEvent<HTMLInputElement>) => setRecordName(event.target.value);
  const handleChangeTarget = (event: React.ChangeEvent<HTMLInputElement>) => setTarget(event.target.value);
  const handleChangePriority = (event: React.ChangeEvent<HTMLInputElement>) => setPriority(parseInt(event.target.value));
  const handleChangeTtl = (event: React.ChangeEvent<HTMLInputElement>) => setTtl(parseInt(event.target.value));
  const handleChangeType = (event: React.ChangeEvent<HTMLInputElement>) => setType(event.target.value);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setIsLoading(true)

    if (!domainIdProp?.domainId) {
      toast.error("Domain ID is missing")
      setIsLoading(false)
      return
    }

    const recordData: CreateRecordsDto = {
      domainId: domainIdProp.domainId,
      recordName: recordName,
      target: target,
      recordType: type,
      priority: priority,
      ttl: ttl,
    }

    try {
      const result = await record.create(recordData)

      if (result)
      {
        toast.success("Record added successfully!")
        
        // Close the dialog
        setIsOpen(false)
        
        // Reset form fields
        setRecordName("")
        setTarget("")
        setPriority(0)
        setTtl(0)
        setType("")
        
        // Refresh the page to show the new record
        navigate(0)
      }
    } catch (error) {
      toast.error(error?.message || "An error occurred while adding the record.")
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button className="mt-4 w-full">
          <PlusCircle className="mr-2" />
          Add Record
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Record</DialogTitle>
            <DialogDescription>
              Add a new record.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid gap-3">
              <Label htmlFor="recordName">Name</Label>
              <Input
                id="recordName"
                name="recordName"
                onChange={handleChangeName}
                required
              />
            </div>
            <div className="grid gap-3">
              <Label htmlFor="target">Target</Label>
              <Input
                id="target"
                name="target"
                onChange={handleChangeTarget}
                required
              />
            </div>
            <div className="grid gap-3">
              <Label htmlFor="type">Type</Label>
              <Input
                id="type"
                name="type"
                onChange={handleChangeType}
                required
              />
            </div>
            <div className="grid gap-3">
              <Label htmlFor="priority">Priority</Label>
              <Input
                id="priority"
                name="priority"
                type="number"
                onChange={handleChangePriority}
                required
              />
            </div>
            <div className="grid gap-3">
              <Label htmlFor="ttl">TTL</Label>
              <Input
                id="ttl"
                name="ttl"
                type="number"
                onChange={handleChangeTtl}
                required
              />
            </div>
          </div>
          <DialogFooter>
            <Button
              type="submit"
              className="w-full"
              disabled={isLoading || !recordName || !target || !type}
            >
              {isLoading ? <Spinner /> : "Add Record"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}